using System.Threading;
using System.Threading.Tasks;

using Teaching.Concurrency.Queue.DataAccess;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class ThreadPoolSchedulerItem
    {
        // Тип сообщения, который будет обрабатываться. Например, Email, Sms или Push.
        // Поле находится в классе ThreadPoolSchedulerItem.
        public MessageQueueItemType Type { get; private set; }

        // Событие для синхронизации потоков. Используется для уведомления о завершении задачи.
        // Поле находится в классе ThreadPoolSchedulerItem.
        public WaitHandle WaitHandle { get; private set; }

        // Конструктор, инициализирующий тип сообщения и создающий событие синхронизации.
        // Метод находится в классе ThreadPoolSchedulerItem.
        public ThreadPoolSchedulerItem(MessageQueueItemType type)
        {
            // Устанавливаем тип сообщения.
            Type = type;

            // Создаем событие AutoResetEvent, которое будет использоваться для управления потоками.
            WaitHandle = new AutoResetEvent(false);
        }
    }
}