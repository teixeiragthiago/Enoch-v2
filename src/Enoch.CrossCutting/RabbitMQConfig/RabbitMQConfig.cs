using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.CrossCutting.RabbitMQConfig
{
    public static class RabbitMQConfig
    {
        private static readonly string _hostname = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME");
        private static readonly string _password = Environment.GetEnvironmentVariable("");
        private static readonly string _queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE_NAME");
        private static readonly string _username = Environment.GetEnvironmentVariable("");
        private static readonly string _port = Environment.GetEnvironmentVariable("RABBITMQ_PORT");

        public static RabbitMQConfigData GetData()
        {
            return new RabbitMQConfigData
            {
                Hostname = _hostname,
                Password = _password,
                QueueName = _queueName,
                UserName = _username,
                Port = _port
            };
        }
    }

    public class RabbitMQConfigData
    {
        public string Hostname { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
        public string UserName { get; set; }
        public string Port { get; set; }
    }
}
