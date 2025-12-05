using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teaching.Concurrency.Queue.Access;
using Teaching.Concurrency.Queue.Handler.MessageHandlers;

namespace Teaching.Concurrency.Queue.Handler.QueueHandlers
{
    public class TypeQueueHandler: IQueueHandler
    {
        private readonly IQueueMessageHandler _messageHandler;
        private readonly MessageQueueItemType _type;

        // Конструктор, принимающий обработчик сообщений и тип сообщений.
        // Метод находится в классе TypeQueueHandler.
        public TypeQueueHandler(IQueueMessageHandler messageHandler, MessageQueueItemType type)
        {
            _messageHandler = messageHandler;
            _type = type;
        }

        // Метод для обработки сообщений определенного типа.
        // Метод находится в классе TypeQueueHandler.
        public void Handle()
        {
            Console.WriteLine($"Handler started for type: {_type} in process: {Process.GetCurrentProcess().Id} " +
                              $"in domain {AppDomain.CurrentDomain.Id} in thread: {Thread.CurrentThread.ManagedThreadId}, " +
                              $"thread from pool: {Thread.CurrentThread.IsThreadPoolThread}");

            using DataContext dataContext = new DataContext();
            // Получаем сообщения из очереди, соответствующие заданному типу.
            var messages = dataContext.MessageQueue.Where(x => x.Type == _type).ToList();
            foreach (var message in messages)
            {
                // Обрабатываем каждое сообщение.
                _messageHandler.Handle(message);
            }
            
            // Удаляем обработанные сообщения из очереди и сохраняем изменения.
            dataContext.MessageQueue.RemoveRange(messages);
            dataContext.SaveChanges();
            
            Console.WriteLine($"Handled messages: {messages.Count}");
        }
    }
}