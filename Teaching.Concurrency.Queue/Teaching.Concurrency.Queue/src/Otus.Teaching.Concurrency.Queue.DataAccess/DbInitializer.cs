using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teaching.Concurrency.Queue.Access
{
    public class DbInitializer
    {
        private readonly Random _random;
        private readonly DataContext _dataContext;
        private const int QueueItemsCount = 1000;

        // Конструктор, инициализирующий контекст данных и генератор случайных чисел.
        // Метод находится в классе DbInitializer.
        public DbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
            _random = new Random();
        }

        // Метод для инициализации базы данных.
        // Метод находится в классе DbInitializer.
        public void Initialize()
        {
            // Удаляем существующую базу данных, если она есть.
            _dataContext.Database.EnsureDeleted();
            // Создаем новую базу данных.
            _dataContext.Database.EnsureCreated();

            // Генерируем сообщения для очереди.
            var queue = new List<MessageQueueItem>();
            var now = DateTime.Now;
            for (int i = 1; i <= QueueItemsCount; i++)
            {
                var message = new MessageQueueItem()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    CreateDate = now.AddMinutes(i),
                    Type = GetRandomMessageType()
                };
                
                queue.Add(message);
            }

            // Добавляем сообщения в базу данных и сохраняем изменения.
            _dataContext.MessageQueue.AddRange(queue);
            _dataContext.SaveChanges();
        }

        // Метод для получения случайного типа сообщения.
        // Метод находится в классе DbInitializer.
        private MessageQueueItemType GetRandomMessageType()
        {
            var typeValue = _random.Next(1, 4);
            return (MessageQueueItemType) typeValue;
        }
    }
}