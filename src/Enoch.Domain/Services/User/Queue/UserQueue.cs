using Amazon.SQS;
using Amazon.SQS.Model;
using Enoch.CrossCutting.AwsSQS;
using Enoch.CrossCutting.LogWriter;
using Enoch.CrossCutting.Notification;
using Enoch.CrossCutting.RabbitMQConfig;
using Enoch.Domain.Services.User.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.Domain.Services.User.Queue
{
    public class UserQueue : IUserQueue
    {
        private readonly INotification _notification;
        private IConnection _connection;
        private readonly IAmazonSQS _sqsAmazon;
        private readonly string _queueName = RabbitMQConfig.GetData().QueueName;
        private readonly int _maxMessages = 1;
        private readonly int _waitTime = 2;

        public UserQueue(INotification notification, IAmazonSQS sqsAmazon)
        {
            _notification = notification;
            _sqsAmazon = sqsAmazon;
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
                    Port = int.Parse(rabbitConfig.Port)
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

        public async Task<bool> SendSqsMessage(UserEntity user)
        {
            try
            {
                var messageQueue = JsonConvert.SerializeObject(user);
                var messageRequest = new SendMessageRequest(AwsConfig.GetSqsConfig().SqsConfig.QueueURL, messageQueue);

                await _sqsAmazon.SendMessageAsync(messageRequest);

                return true;
            }
            catch (Exception e)
            {
                LogWriter.WriteError($"Erro ao tentar enviar mensagem para a fila {_queueName} do SQS : {e.Message}");
                return false;
            }
        }

        public async Task<ReceiveMessageResponse> ReceiveSqsMessage()
        {
            try
            {
                var message = await _sqsAmazon.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = AwsConfig.GetSqsConfig().SqsConfig.QueueURL,
                    MaxNumberOfMessages = _maxMessages,
                    WaitTimeSeconds = _waitTime,
                });

                return message;
            }
            catch (Exception e)
            {
                LogWriter.WriteError($"Erro: {e.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteSqsMessage()
        {
            try
            {
                var message = ReceiveSqsMessage();
                if (message != null)
                    await _sqsAmazon.DeleteMessageAsync(AwsConfig.GetSqsConfig().SqsConfig.QueueURL, message.Result.Messages.FirstOrDefault()?.ReceiptHandle);

                return true;
            }
            catch (Exception e)
            {
                LogWriter.WriteError($"Erro: {e.Message}");
                return false;
            }
        }
    }
}
