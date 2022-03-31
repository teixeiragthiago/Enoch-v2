using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.CrossCutting.ServiceBusConfig
{
    public class ServiceBusConfig
    {
        private static readonly string _primaryKey = Environment.GetEnvironmentVariable("SERVICE_BUS_PRIMARY_KEY");
        private static readonly string _primaryConnectionString =  Environment.GetEnvironmentVariable("SERVICE_BUS_PRIMARY_CONNSTRING");
        private static readonly string _queueName =  Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE");
        private static readonly string _topicName =  Environment.GetEnvironmentVariable("SERVICE_BUS_TOPIC");

        public static ServiceBusConfigData GetData()
        {
            return new ServiceBusConfigData
            {
                ConnectionString = _primaryConnectionString,
                PrimaryKey = _primaryKey,
                QueueName = _queueName,
                TopicName = _topicName
            };
        }
    }

    public class ServiceBusConfigData
    {
        public string PrimaryKey { get; set; }
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string TopicName { get; set; }
    }
}
