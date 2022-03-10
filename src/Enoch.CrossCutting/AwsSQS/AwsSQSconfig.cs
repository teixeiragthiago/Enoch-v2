using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.CrossCutting.AwsSQS
{
    public static class AwsSQSconfig
    {
        private static readonly string _client = Environment.GetEnvironmentVariable("AWS_SQS_QUEUE_NAME");
        private static readonly string _urlQueue = Environment.GetEnvironmentVariable("AWS_SQS_QUEUE_URL");

        public static SQSDataConfig GetSqsConfig()
        {
            return new SQSDataConfig
            {
                SqsClient = _client,
                QueueURL = _urlQueue
            };
        }
    }

    public class SQSDataConfig
    {
        public string SqsClient { get; set; }
        public string QueueURL { get; set; }
    }
}
