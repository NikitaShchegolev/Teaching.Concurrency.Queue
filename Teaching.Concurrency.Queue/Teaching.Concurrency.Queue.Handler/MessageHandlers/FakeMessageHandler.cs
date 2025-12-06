using System.Threading;

using Teaching.Concurrency.Queue.DataAccess;
using Teaching.Concurrency.Queue.Handler;

namespace Teaching.Concurrency.Queue.Handler.MessageHandlers
{
    public class FakeMessageHandler : IQueueMessageHandler
    {
        private readonly int _delay;

        // Конструктор, принимающий задержку обработки
        public FakeMessageHandler(int delay)
        {
            _delay = delay;
        }
        
        // Метод для обработки сообщения с искусственной задержкой
        public void Handle(MessageQueueItem item)
        {
            // Работа по обработке сообщения, например, отправка в другой сервис
            Thread.Sleep(_delay);
        }
    }
}