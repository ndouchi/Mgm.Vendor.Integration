using System;
using System.Collections.Generic;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Helpers;
using Microsoft.Extensions.Options;

namespace SqsSenderApp
{
    class Program
    {
        private string q = "https://sqs.us-west-1.amazonaws.com/073897193669/ServiceRequestTest.fifo";
        private string dq = "https://sqs.us-west-1.amazonaws.com/073897193669/ServiceRequestTestDeadLetter.fifo";

        private readonly IOptions<AccessHelper> _options;
        private SQSHelper _helper = null;
        private string _defaultMessageGroupId = "123456"; //This can be a vendorId if we choose to go with one queue for all vendors
        private string messageGroupId = "";
        public SQSHelper Helper
        {
            get
            {
                if (_helper == null)
                {
                    var kv = (AccessHelper)_options.Value;
                    // _helper = new SQSHelper(kv.AccessKey, kv.SecretKey, RegionEndpoint.APNortheast1);
                    _helper = new SQSHelper(kv.AccessKey, kv.SecretKey, RegionEndpoint.USWest1);
                }
                return _helper;
            }
        }
        static void Main(string[] args)
        {
     
        }
  
        private static void Work()
        {
            Console.WriteLine("Amazon SQS");

            var sqsConfig = new AmazonSQSConfig();
            sqsConfig.ServiceURL = "https://sqs.us-west-1.amazonaws.com";
            sqsConfig.RegionEndpoint = RegionEndpoint.USWest1;

            IAmazonSQS sqs = new AmazonSQSClient(RegionEndpoint.USWest1);

            Console.WriteLine("Create a queue...\n");
            var attrs = new Dictionary<string, string>();
            attrs.Add(QueueAttributeName.VisibilityTimeout, "10");

            var sqsRequest = new CreateQueueRequest
            {
                QueueName = "mgmQueue",
                Attributes = attrs

            };

            var createQueueResponse = sqs.CreateQueueAsync(sqsRequest).Result;

            var myQueueUrl = createQueueResponse.QueueUrl;

            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueuesAsync(listQueuesRequest);

            Console.WriteLine("List of Amazon SQS queues.\n");
            foreach (var queueUrl in listQueuesResponse.Result.QueueUrls)
            {
                Console.WriteLine($"QueueUrl: {queueUrl}");

            }

            Console.WriteLine("Send a message to the queue.\n");
            var sqsMessageRequest = new SendMessageRequest
            {
                QueueUrl = myQueueUrl,
                MessageBody = "Request Info"
            };

            sqs.SendMessageAsync(sqsMessageRequest);

            Console.WriteLine("Message Sent To The Queue.\n");
            Console.ReadLine();
        }
    }
}
