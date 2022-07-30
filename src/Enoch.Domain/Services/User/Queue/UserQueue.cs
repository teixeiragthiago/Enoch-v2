using Enoch.CrossCutting;
using Enoch.CrossCutting.LogWriter;
using Enoch.CrossCutting.Notification;
using Enoch.CrossCutting.RabbitMQConfig;
using Enoch.Domain.Services.User.Entities;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message = Microsoft.Azure.ServiceBus.Message;

namespace Enoch.Domain.Services.User.Queue
{
    public class UserQueue : IUserQueue
    {
        private readonly INotification _notification;
        private IConnection _connection;
        private readonly string _queueName = RabbitMQConfig.GetData().QueueName;

        public UserQueue(INotification notification)
        {
            _notification = notification;
        }

        public bool SendQueue(UserEntity user)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(user);
                    var message = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: message);


                    return true;
                }
            }
            else
                return _notification.AddWithReturn<bool>($"Não foi possível encontrar conexão com o RabbitMQ!");
        }

        private void CreateConnection()
        {
            try
            {
                var rabbitConfig = RabbitMQConfig.GetData();

                var factory = new ConnectionFactory
                {
                    HostName = rabbitConfig.Hostname,
                    Port = int.Parse(rabbitConfig.Port),
                    UserName = rabbitConfig.UserName,
                    Password = rabbitConfig.Password
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                LogWriter.WriteError($"Erro ao tentar criar a conexão com o RabbitMQ! : {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;

            CreateConnection();

            return _connection != null;
        }
    }
}
