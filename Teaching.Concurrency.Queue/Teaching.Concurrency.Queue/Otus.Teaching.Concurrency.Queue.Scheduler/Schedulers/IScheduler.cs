namespace Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public interface IScheduler
    {
        // Метод для запуска обработки очереди.
        //1. Инициализирует задачи для обработки сообщений.
        //2. Запускает обработку задач (в зависимости от реализации).
        //3. Ожидает завершения всех задач.
        // Метод находится в интерфейсе IScheduler.
        void ProcessQueue();
    }
}