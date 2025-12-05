namespace Teaching.Concurrency.Queue.Access
{
    // Перечисление типов сообщений в очереди
    public enum MessageQueueItemType
    {
        Email = 1, // Электронная почта
        Sms = 2, // СМС-сообщение
        Push = 3 // Push-уведомление
    }
}