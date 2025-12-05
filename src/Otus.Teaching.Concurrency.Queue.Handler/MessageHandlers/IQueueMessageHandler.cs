using Teaching.Concurrency.Queue.Access;

namespace Teaching.Concurrency.Queue.Handler.MessageHandlers
{
    public interface IQueueMessageHandler
    {
        // Метод для обработки сообщения из очереди.
        //1. Получает объект сообщения `MessageQueueItem`.
        //2. Выполняет обработку сообщения (например, отправка Email, Sms или Push).
        // Метод находится в интерфейсе IQueueMessageHandler.
        void Handle(MessageQueueItem item);
    }
}