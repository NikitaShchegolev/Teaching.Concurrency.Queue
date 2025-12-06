using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Teaching.Concurrency.Queue.DataAccess;
using Teaching.Concurrency.Queue.Handler.MessageHandlers;
using Teaching.Concurrency.Queue.Handler.QueueHandlers;

namespace Teaching.Concurrency.Queue.Handler
{
    public class QueueHandlerFactory
    {
        private const int HandlingDelayMilliseconds = 10;

        // Метод для создания простого обработчика очереди
        public static IQueueHandler GetSimpleQueueHandler()
        {
            return new SimpleQueueHandler(new FakeMessageHandler(HandlingDelayMilliseconds));
        }
        
        // Метод для создания обработчика очереди для определенного типа сообщений
        public static IQueueHandler GetTypeQueueHandler(MessageQueueItemType type)
        {
            return new TypeQueueHandler(new FakeMessageHandler(HandlingDelayMilliseconds), type);
        }

        // Метод для создания нового соединения с RabbitMQ
        public static IConnection CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost" // Укажите хост вашего RabbitMQ
            };
            return factory.CreateConnection();
        }

        // Метод для создания потребителя сообщений RabbitMQ
        public static EventingBasicConsumer CreateConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                // Логика обработки входящих сообщений
                Console.WriteLine("Message received: " + System.Text.Encoding.UTF8.GetString(ea.Body.ToArray()));
            };
            return consumer;
        }
    }
}