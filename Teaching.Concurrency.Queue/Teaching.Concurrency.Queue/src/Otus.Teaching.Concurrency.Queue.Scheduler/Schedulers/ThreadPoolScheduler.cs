using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Teaching.Concurrency.Queue.Access;
using Teaching.Concurrency.Queue.Handler;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    class ThreadPoolScheduler
        : IScheduler
    {
        private readonly Dictionary<MessageQueueItemType,ThreadPoolSchedulerItem> _threadPoolSchedulerItems = 
            new Dictionary<MessageQueueItemType, ThreadPoolSchedulerItem> 
        {
            {MessageQueueItemType.Email, new ThreadPoolSchedulerItem(MessageQueueItemType.Email)},
            {MessageQueueItemType.Sms, new ThreadPoolSchedulerItem(MessageQueueItemType.Sms)},
            {MessageQueueItemType.Push, new ThreadPoolSchedulerItem(MessageQueueItemType.Push)},
        };
        
        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();

            Console.WriteLine($"Thread pool scheduler in thread: {Thread.CurrentThread.ManagedThreadId}...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();

            /**/
            // Получаем количество занятых потоков в пуле потоков
            ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
            Console.WriteLine($"Min worker threads: {minWorkerThreads}, Min completion port threads: {minCompletionPortThreads}");

            // Получаем количество потоков в пуле потоков
            ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads); 
            Console.WriteLine($"Max worker threads: {workerThreads}, Max completion port threads: {completionPortThreads}");

            // Получаем количество свободных потоков в пуле потоков
            ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableCompletionPortThreads);
            Console.WriteLine($"Available worker threads: {availableWorkerThreads}, Available completion port threads: {availableCompletionPortThreads}");
            /**/

            ThreadPool.QueueUserWorkItem(HandleInThreadPool, _threadPoolSchedulerItems[MessageQueueItemType.Email]);
            ThreadPool.QueueUserWorkItem(HandleInThreadPool, _threadPoolSchedulerItems[MessageQueueItemType.Sms]);
            ThreadPool.QueueUserWorkItem(HandleInThreadPool, _threadPoolSchedulerItems[MessageQueueItemType.Push]);

            WaitHandle[] waitHandles = _threadPoolSchedulerItems.Values.Select(x => x.WaitHandle).ToArray();
            
            WaitHandle.WaitAll(waitHandles);
            
            stopWatch.Stop();

            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }
        
        private void HandleInThreadPool(object item)
        {
            var schedulerItem = (ThreadPoolSchedulerItem) item;
            var handler = QueueHandlerFactory.GetTypeQueueHandler(schedulerItem.Type);
            
            handler.Handle();

            var autoResetEvent = (AutoResetEvent) schedulerItem.WaitHandle;
            
            autoResetEvent.Set();
        }
    }
}
