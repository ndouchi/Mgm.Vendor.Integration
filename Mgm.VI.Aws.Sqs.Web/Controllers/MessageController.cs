using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Helpers;
using Mgm.VI.Aws.Sqs.Models;
using Mgm.VI.Aws.Sqs.Rules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mgm.VI.Business;
using Mgm.VI.Common;
using Mgm.VI.Data.Dto;
using Mgm.VI.Logger;

namespace Mgm.VI.Aws.Sqs.Web.Controllers
{
    public class MessageController : Controller
    {
        private static readonly string logs = @"C:\Temp\Mgm.VI.Aws.Sqs.Processor\Logs\";
        private static readonly ILoggerService Logger = new LoggerService(LoggingMode.ToDatabaseAndFile, logs);
        #region imported from Processor
        private static Microsoft.Extensions.Configuration.IConfiguration Configuration;
        //private string _defaultMessageGroupId = "123456"; //This can be a vendorId if we choose to go with one queue for all vendors
        //private string messageGroupId = "";
        private static AwsUser _awsUser;
        private static Amazon.SQS.AmazonSQSConfig _sqsConfig;
        private static List<IRule> _rules;
        private static IVendorDto vendor = new VendorDto { VendorId = "vubiquityId" };// To be read from DB or commandline arguments
        private static Dto.SqsQueueDto _messageQueue, _deadLetterQueue;
        private static string _messageGroupId = String.Empty;
        private static bool _queueWithMessageGroupId = false;
        private static readonly string storageDirectory = @"C:\Temp\Mgm.VI.Aws.Sqs.Processor\Data\";
        private static readonly string connectionString = "Server=AWINANC3,4451;Database=MSS_STAGING;Trusted_Connection=True;";
        private static readonly Storage storage = new Storage(StorageType.Database, storageDirectory, connectionString);
        #endregion  imported from Processor


        private readonly IOptions<AccessHelper> _options;
        private SQSHelper _helper = null;
        private string xsdFilePath = "";
        public static IRulesInspector RulesInspector { get; set; }

        public MessageController(IOptions<AccessHelper> options)
        {
            _options = options;
            var kv = (AccessHelper)_options.Value;
            //var access = Configuration.GetSection("Access");
            //var accessKey =  access["AccessKey"];
            //var secretKey = access["SecretKey"];
            _awsUser = new AwsUser(kv.AccessKey, kv.SecretKey);
            _sqsConfig = new AmazonSQSConfig()
            {
                ServiceURL = "https://sqs.us-west-1.amazonaws.com",
                RegionEndpoint = RegionEndpoint.USWest1
            };
        }
        public SQSHelper Helper
        {
            get
            {
                if (_helper == null)
                {
                    var kv = (AccessHelper)_options.Value;
                    _helper = new SQSHelper(kv.AccessKey, kv.SecretKey, RegionEndpoint.USWest1);
                }
                return _helper;
            }
        }
        public async Task<IActionResult> Index()
        {
            var models = new List<QueueModel>();
            var queues = await Helper.GetQueue();
            foreach (var q in queues.QueueUrls)
            {
                models.Add(new QueueModel(q));
            }
            return View(models);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(QueueModel model)
        {
            if (ModelState.IsValid)
            {
                await Helper.CreateQueue(model.QueueName);

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete()
        {
            var request = Request.Query["query"].ToString();
            if (request == null)
                throw new InvalidOperationException();
            await Helper.DeleteQueue(request.ToString());
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Send()
        {
            var url = Request.Query["url"].ToString();
            if (url == null)
                throw new InvalidOperationException();
            var model = new MessageModel("", url, "");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Send(MessageModel model)
        {
            if (ModelState.IsValid)
            {
                await Helper.SendMessage(model.URL, model.Message, "12345");
            }
            return RedirectToAction("messages", new { url = model.URL });
        }

        public async Task<IActionResult> Messages()
        {
            var url = Request.Query["url"].ToString();
            var response = await Helper.GetAllMessages(url);
            var models = new List<MessageModel>();
            foreach (var msg in response)
            {
                models.Add(new MessageModel(msg.Body, url, msg.ReceiptHandle));
            }
            return View(models);
        }
        private static void SetUpQueues()
        {
            _deadLetterQueue = new SqsQueueDto("MgmVendorDeadLetter.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/MgmVendorDeadLetter.fifo", QType.Fifo);
            _messageQueue = new SqsQueueDto("MgmQueue.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/MgmVendor.fifo", QType.Fifo, _deadLetterQueue);
        }
        public async Task<IActionResult> ProcessMessages()
        {
            var url = Request.Query["url"].ToString();
            SetUpQueues();

            var vsu = new VendorStatusUpdate(   storage,
                                                vendor,
                                                xsdFilePath,
                                                _messageQueue, 
                                                _deadLetterQueue,
                                                _awsUser, 
                                                _sqsConfig, 
                                                _queueWithMessageGroupId,
                                                _messageGroupId,
                                                Logger,
                                                RulesInspector);
            await vsu.ProcessVendorUpdates();// _messageQueue, _deadLetterQueue, _awsUser, _sqsConfig, _rules);
            var response = await Helper.GetNextMessage(_messageQueue, _deadLetterQueue, _awsUser, _sqsConfig, RulesInspector);
            //var response = await Helper.GetAllMessages(url);

            var models = new List<MessageModel>();
            var msg = response;
            if (msg != null)
                models.Add(new MessageModel(msg.Body, url, msg.ReceiptHandle));

            return View(models);
        }
        public async Task<IActionResult> DeleteMessage()
        {
            var url = Request.Query["url"].ToString();
            var handler = Request.Query["handler"].ToString();
            await Helper.DeleteMessageAsync(url, handler);
            return RedirectToAction("Index");
        }

    }
}