using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.CrossCutting.AwsSQS
{
    public static class AwsConfig
    {
        private static readonly string _client = Environment.GetEnvironmentVariable("AWS_SQS_QUEUE_NAME");
        private static readonly string _urlQueue = Environment.GetEnvironmentVariable("AWS_SQS_QUEUE_URL");
        private static readonly int _maxMessages = int.Parse(Environment.GetEnvironmentVariable("AWS_QUEUE_MAX_MESSAGES"));
        private static readonly double _waitTime = double.Parse(Environment.GetEnvironmentVariable("AWS_QUEUE_WAIT_TIME"));
        private static readonly string _bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET");

        public static AwsConfigData GetSqsConfig()
        {
            return new AwsConfigData
            {
                SqsConfig = new SqsDataConfig
                {
                    SqsClient = _client,
                    QueueURL = _urlQueue,
                    MaxMessages = _maxMessages,
                    WaitTime = _waitTime
                },
                AwsS3Config = new AwsS3DataConfig
                {
                    BucketName = _bucketName
                }
            };
        }
    }

    public sealed class AwsConfigData
    {
        public SqsDataConfig SqsConfig { get; set; }
        public AwsS3DataConfig AwsS3Config { get; set; }
    }

    public sealed class SqsDataConfig
    {
        public string SqsClient { get; set; }
        public string QueueURL { get; set; }
        public int MaxMessages { get; set; }
        public double WaitTime { get; set; }
    }

    public sealed class AwsS3DataConfig
    {
        public string BucketName { get; set; }
    }

}
