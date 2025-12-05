using System.Linq;
using System.Linq;

using Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.DataAccess;
using Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.Handler.MessageHandlers;

namespace Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers
{
    public class SimpleQueueHandler : IQueueHandler
    {
        private readonly IQueueMessageHandler _messageHandler;

        // Конструктор, принимающий обработчик сообщений
        public SimpleQueueHandler(IQueueMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        // Метод для обработки всех сообщений в очереди
        public void Handle()
        {
            using DataContext dataContext = new DataContext();
            // Преобразуем в список для возможности безопасного удаления элементов
            var messages = dataContext.MessageQueue.ToList();
            foreach (var message in messages)
            {
                // Обработка сообщения
                _messageHandler.Handle(message);
            }
            
            // Удаление обработанных сообщений из очереди
            dataContext.MessageQueue.RemoveRange(messages);
            
            // Сохранение изменений в базе данных
            dataContext.SaveChanges();
        }
    }
}