//////////////////////////////////////////////////////////////////
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   10/11/2019 
/// 
//////////////////////////////////////////////////////////////////

using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws;
using Mgm.VI.Aws.Sqs;
using Mgm.VI.Aws.Sqs.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Repository;
using Mgm.VI.Common;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Business;
using System.Xml.Linq;
using Mgm.VI.Data.Dto;
using Mgm.VI.Logger;

namespace Mgm.VI.Aws.Sqs.Processor
{
    class Program
    {
        #region Private Vars
        private static string _vendorId = "vubiquity"; // Get this from a configuration file
        private static IConfiguration Configuration;
        private static System.Configuration.Configuration MyConfiguration;
        private static AwsUser _awsUser;
        private static AmazonSQSConfig _sqsConfig;
        private static IVendorDto vendorDtoTemp = new VendorDto {  StagingId = 0,
                                                            VendorId = "vubiquity", 
                                                            VendorName = "Vubiquity, Inc.",
                                                            ServiceRequestApiURI = "",
                                                            SQS_StatusUpdatePrimaryURI = "",
                                                            SQS_StatusUpdateDeadLetterURI = "",
                                                            SQS_ServiceURL = "https://sqs.us-west-1.amazonaws.com",
                                                            SQS_RegionEndPoint = "RegionEndpoint.USWest1",
                                                            AWS_AccessKey = "AKIARCNE5WDC55GZE4N5",
                                                            AWS_SecretKey = "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"
                                                         };
        private static string xmlRecordContent;
        private static string xsdFilePath = @"C:\Code\Mgm.VI\UnitTests\Mgm.VI.Data.Dto.UnitTests\TestData\ServiceRequest_Schema.xsd";// @"C:\Code\Mgm.VI\UnitTestsMgm.VI.Aws.Sqs.Processor\Data\ServiceRequest_Schema.xsd";
        private static IRulesInspector _rulesInspector;
        private static List<IRule> _rules;
        private static List<IVendorDto> vendorDtos;
        private static IVendorDto vendorDto;

        private static SqsQueueDto _messageQueue, _deadLetterQueue;
        private static string _messageGroupId = String.Empty;
        private static bool _queueWithMessageGroupId = false;
        private static int _charsToDisplay = 80;
        private static readonly string logsDir = @"C:\Temp\Mgm.VI.Aws.Sqs.Processor\Logs\";
        private static readonly string storageDirectory = @"C:\Temp\Mgm.VI.Aws.Sqs.Processor\Data\";
        private static readonly string connectionString = "Server=AWINANC3,4451;Database=MSS_STAGING;Trusted_Connection=True;";
        private static readonly Storage storage = new Storage(StorageType.Database, storageDirectory, connectionString);
        private static readonly ILoggerService Logger = new LoggerService(LoggingMode.ToDatabaseAndFile, logsDir);
        #endregion Private Vars
        enum ProcessingOrderEnum
        {
            Normal = 0,
            Reverse = 1
        }
        static void Main(string[] args)
        {
            RunProcess();
        }
        private static void RunProcess()
        {
            bool rerun = true;

            while (rerun)
            {
                #region Entire Process
                Introduction();
                if (GetConfigurations())
                {
                    #region Process Core
                    IQueueAccess qa = ConstructQueueAccess();
                    var vsu = new VendorStatusUpdate(storage,
                                                        vendorDto, //Consider making this a list of vendors
                                                        xsdFilePath,
                                                        Logger,
                                                        qa);
                    PostProcessing(vsu);
                    rerun = GetBinaryOption("\n\n\n ===> Do you want to exit or re-run the entire process? [Enter 0 to rerun, 1 to exit]: ", "\n\n\n", ConsoleColor.Red);
                    #endregion Process Core
                }
                else
                {
                    #region Failed Retrieving Configuration
                    rerun = false;
                    Display("Due to failure in retrieving essential configuration, the application is powering down.", true, ConsoleColor.Red);
                    #endregion Failed Retrieving Configuration
                }
                #endregion Entire Process
            }
        }
        private static void Introduction()
        {
            #region Display Intro
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Display("Launching the ", false, ConsoleColor.Green);
            Display(" Mgm.VI.Aws.Sqs.Processor ", false, ConsoleColor.Yellow);
            Display(" application...", true, ConsoleColor.Green);
            #endregion Display Intro
            Display("Retrieving Application Configuration...", true, ConsoleColor.Green);
        }
        private static void Display(string text, bool newline = true,
                                 ConsoleColor fgColor = ConsoleColor.White,
                                 ConsoleColor bgColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.Write("{0}{1}", text, newline ? "\n" : "");
            Console.ResetColor();
        }
        private static void DisplayMessages(string intro,
                                            List<Message> messages,
                                            bool displayAllContent = true,
                                            ConsoleColor fgColor = ConsoleColor.White,
                                            ConsoleColor bgColor = ConsoleColor.Black)
        {
            bool alternateColor = false;
            if (messages.Count > 0)
            {
                Display(intro);
                messages.ForEach(m => DisplayMessage(m, displayAllContent, ref alternateColor, fgColor, bgColor));
            }
        }
        private static void DisplayMessage(Message message, 
                                            bool displayAllContent,
                                            ref bool alternateColor,
                                            ConsoleColor fgColor = ConsoleColor.White, 
                                            ConsoleColor bgColor = ConsoleColor.Black
                                            )
        {
            var messageBodyLength = message.Body.Length;
            var messageBody = displayAllContent ? message.Body :
                                                    message.Body?.Substring(0,
                                                    messageBodyLength > _charsToDisplay
                                                                        ? _charsToDisplay : messageBodyLength);
            if (!displayAllContent && messageBodyLength > _charsToDisplay) messageBody += "...";
            Display(messageBody, true, fgColor, alternateColor? ConsoleColor.DarkGray : bgColor);
            alternateColor = !alternateColor;
        }
        private static void DisplayStats(VendorStatusUpdate vsu, bool displayAllContent = false)
        {
            Display("Completed ProcessVendorUpdates...\n\n", true, ConsoleColor.Green);
            Display(String.Format("{0} ", vsu.FetchedMessages.Count), false, ConsoleColor.DarkYellow);
            Display("messages were VIEWED from the queue.", true, ConsoleColor.Blue);
            Display(String.Format("{0} ", vsu.ProcessedMessages.Count), false, ConsoleColor.DarkGreen);
            Display("messages were SUCCESSFULLY processed.", true, ConsoleColor.Blue);
            Display(String.Format("{0} ", vsu.DeadLetterMessages.Count), false, ConsoleColor.DarkRed);
            Display("messages were MOVED to the Dead Letter Queue.\n", true, ConsoleColor.Blue);

            DisplayMessages("The following messages were fetched from the queue", vsu.FetchedMessages, displayAllContent);
            DisplayMessages("The following messages were processed successfully", vsu.ProcessedMessages, displayAllContent, ConsoleColor.Green);
            DisplayMessages("The following messages couldn't be processed and were moved to the dead letter queue", vsu.DeadLetterMessages, displayAllContent, ConsoleColor.Red);
        }
        private static void DisplayWaitingText(Task updateProcess)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Working...");
            while (!updateProcess.IsCompleted)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        private static bool GetBinaryOption(string promptMsg, string trailingText = "", ConsoleColor fgColor = ConsoleColor.White)
        {
            if (null == promptMsg)
            {
                throw new ArgumentNullException("ERROR: promptMsg cannot be null.");
            }

            bool binaryValue = false;
            string inputStr = string.Empty;

            int num = -1;
            while (num != (int)ProcessingOrderEnum.Normal && num != (int)ProcessingOrderEnum.Reverse)
            {
                Display(promptMsg, false, fgColor);
                try
                {
                    inputStr = Console.ReadLine();
                    int.TryParse(inputStr, out num);
                }
                catch (Exception ex)
                {
                    Display(" ------------ ERROR ------------ ", true, ConsoleColor.Red);
                    Display(ex.Message, true, ConsoleColor.Red);
                    Display(" ------------ ERROR ------------ ", true, ConsoleColor.Red);
                }
            }
            binaryValue = num == 0 ? true : false;

            if (!string.IsNullOrEmpty(trailingText))   Display(trailingText, true, fgColor);

            return binaryValue;
        }
        private static System.Configuration.ConfigurationSection GetCustomSection(string sectionName)
        {
            try
            {
                System.Configuration.ConfigurationSection customSection;

                // Get the current configuration file.
                System.Configuration.Configuration config =
                        ConfigurationManager.OpenExeConfiguration(
                        ConfigurationUserLevel.None) as Configuration;

                customSection =
                    config.GetSection(sectionName) as System.Configuration.ConfigurationSection;

                return customSection;
            }
            catch (ConfigurationErrorsException err)
            {
                Console.WriteLine("Using GetSection(string): {0}", err.ToString());
                return null;
            }
        }
        private static bool GetConfigurations()
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                        
                                        .Build();
                //TODO: REMOVE THIS LINE AND RESTORE THE ONE BELOW IT
                var mssStagingDatabase = "Server=(NOURD01-PO\\MSSQLSERVER2019D);Database=MSS_STAGING;Trusted_Connection=True;";

                //TODO: RESTORE: var connectionStrings = Configuration.GetSection("ConnectionStrings").GetChildren();
                //var mssStagingDatabase = connectionStrings["MssStagingDatabase"];
                //var mssStagingDatabase = GetCustomSection("ConnectionStrings");


                bool useFakeConfiguration = false;
                try
                {
                    //var mockOptions = Configuration.GetSection("MockOptions");
                    var useFakeConfig = "false";// mockOptions["UseFakeConfiguration"];
                    bool.TryParse(useFakeConfig?.ToString(), out useFakeConfiguration);
                }
                catch (Exception e)
                {
                    Display("Warning: ", false, ConsoleColor.Magenta);
                    Display("the MockOptions couldn't be retrieved.  The application will proceed as if no mocking is intended to be used.", true, ConsoleColor.Magenta);
                    Display(string.Format("{0}", e.Message), true, ConsoleColor.Magenta);
                }
                useFakeConfiguration = true;
                var vManager = new Mgm.VI.Business.VendorManager(storage);

                if (useFakeConfiguration)
                {
                    #region temporary code - remove once configuration and rules are set up
                    var access = Configuration.GetSection("Access");
                    var accessKey = access["AccessKey"];
                    var secretKey = access["SecretKey"];
                    _awsUser = new AwsUser(accessKey, secretKey);
                    _awsUser = new AwsUser()
                    {
                        AccessKey = "AKIARCNE5WDC55GZE4N5",
                        SecretKey = "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"
                    };
                    _sqsConfig = new AmazonSQSConfig()
                    {
                        ServiceURL = "https://sqs.us-west-1.amazonaws.com",
                        RegionEndpoint = RegionEndpoint.USWest1
                    };
                    #endregion temporary code - remove once configuration and rules are set up
                }
                else
                {
                    vendorDto = vManager.GetVendor(_vendorId);
                    _sqsConfig = new AmazonSQSConfig()
                    {
                        ServiceURL = vendorDto.SQS_ServiceURL,
                        RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(vendorDto.SQS_RegionEndPoint)
                    };
                    _awsUser = new AwsUser(vendorDto.AWS_AccessKey, vendorDto.AWS_SecretKey);
                }

                _rules = new List<IRule>();
                _rulesInspector = new RulesInspector();

                var processQueuesInReverse = GetBinaryOption("For demo, process messages from Queue to Dead-Letter Queue [0 for normal, 1 for reverse]: ");
                Display("Collect the validation rules for SQS messages...", true, ConsoleColor.Yellow);
                SetTheValidationRules(xsdFilePath, storage);
                Display("Configuring The Queues...", true, ConsoleColor.Green);
                SetUpQueues(processQueuesInReverse);
                Display("Instantiating The VendorStatusUpdate Process...", true, ConsoleColor.Yellow);

                return true;
            }
            catch (Exception e)
            {
                Display("FATAL ERROR: The application encountered the following error when loading the essential configuration.", true, ConsoleColor.Red);
                Display(String.Format("{0}", e.Message), true, ConsoleColor.Red);
                return false;
            }
        }
         private static void PostProcessing(VendorStatusUpdate vsu)
        {
            Display("Starting ProcessVendorUpdates...", true, ConsoleColor.Yellow);
            var updateProcess = vsu.ProcessVendorUpdates();
            DisplayWaitingText(updateProcess);
            DisplayStats(vsu);
        }

        private static IQueueAccess ConstructQueueAccess()
        {
            IQueueAccess qa = new QueueAccess(_awsUser,
                                              _sqsConfig,
                                              _messageQueue,
                                              _deadLetterQueue,
                                              _queueWithMessageGroupId,
                                              _messageGroupId,
                                              Logger,
                                              _rulesInspector);
            return qa;
        }
        private static void SetTheValidationRules(string xsdFilePath, Storage storage)
        {
            var rule = new SqsRule_ValidateXsd(xsdFilePath);
            _rules.Add(rule);
            using (IServiceRequestStatusUnitOfWork srsUow = new ServiceRequestStatusUnitOfWork(storage))
            {
                var rule2 = new ServiceRequestStatusRule(xsdFilePath, srsUow);
                _rules.Add(rule2);
            }
            _rulesInspector.Rules = _rules;
        }
        private static void SetUpQueues(bool fromQ2DeadQueue = true)
        {
            if (fromQ2DeadQueue)
            {
                _deadLetterQueue = new SqsQueueDto("MgmVendorDeadLetter.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/MgmVendorDeadLetter.fifo", QType.Fifo);
                _messageQueue = new SqsQueueDto("MgmQueue.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/MgmVendor.fifo", QType.Fifo, _deadLetterQueue);
            }
            else
            {
                _messageQueue = new SqsQueueDto("MgmVendorDeadLetter.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/MgmVendorDeadLetter.fifo", QType.Fifo);
                _deadLetterQueue = new SqsQueueDto("MgmQueue.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/MgmVendor.fifo", QType.Fifo, _deadLetterQueue);
            }
        }
    }
}
