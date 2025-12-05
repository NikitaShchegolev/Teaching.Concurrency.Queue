using System.Threading;
using System.Threading.Tasks;

namespace Teaching.Concurrency.Queue.Handler.QueueHandlers
{
    public interface IQueueHandler
    {
        // Метод для обработки очереди сообщений.
        //1. Получает очередь сообщений из источника данных.
        //2. Выполняет обработку каждого сообщения в очереди.
        //3. Удаляет обработанные сообщения из очереди.
        // Метод находится в интерфейсе IQueueHandler.
        void Handle();
    }
}