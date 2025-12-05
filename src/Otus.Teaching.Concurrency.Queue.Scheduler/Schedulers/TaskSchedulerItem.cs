using System.Threading.Tasks;
using Teaching.Concurrency.Queue.Access;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class TaskSchedulerItem
    {
        // Тип сообщения, который будет обрабатываться. Например, Email, Sms или Push.
        // Поле находится в классе TaskSchedulerItem.
        public MessageQueueItemType Type { get; set; }

        // Задача, связанная с обработкой сообщения. Хранит ссылку на текущую задачу.
        // Поле находится в классе TaskSchedulerItem.
        public Task Task { get; set; }

        // Количество попыток выполнения задачи. Используется для контроля числа повторных запусков.
        // Поле находится в классе TaskSchedulerItem.
        public int RetryCount { get; set; }

        // Конструктор, инициализирующий тип сообщения.
        // Метод находится в классе TaskSchedulerItem.
        public TaskSchedulerItem(MessageQueueItemType type)
        {
            // Устанавливаем тип сообщения.
            Type = type;
        }
    }
}