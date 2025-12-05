using System;

namespace Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.DataAccess
{
    public class MessageQueueItem
    {
        // Уникальный идентификатор сообщения
        public Guid Id { get; set; }

        // Дата создания сообщения
        public DateTime CreateDate { get; set; }

        // Идентификатор пользователя, связанного с сообщением
        public Guid UserId { get; set; }

        // Тип сообщения
        public MessageQueueItemType Type { get; set; }

        // Переопределение метода ToString для удобного отображения информации о сообщении
        public override string ToString()
        {
            return $"Message. Id: {Id}, Type: {Type}, Created date: {CreateDate}, User Id: {UserId}";
        }
    }
}