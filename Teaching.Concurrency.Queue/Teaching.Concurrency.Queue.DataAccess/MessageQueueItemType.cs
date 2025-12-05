namespace Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.DataAccess
{
    // Перечисление типов сообщений в очереди
    public enum MessageQueueItemType
    {
        Email = 1, // Электронная почта
        Sms = 2, // СМС-сообщение
        Push = 3 // Push-уведомление
    }
}