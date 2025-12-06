using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Teaching.Concurrency.Queue.DataAccess;
using Teaching.Concurrency.Queue.Handler;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class ThreadsBackgroundForegroundScheduler
        : IScheduler
    {
        readonly List<Thread> _queueHandlerThreads = new List<Thread>();

        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();

            Console.WriteLine($"Thread scheduler in thread: {Thread.CurrentThread.ManagedThreadId}...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();

            StartHandlerThreadsAsBackgroundWithoutWaiting();

            stopWatch.Stop();

            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }

        /// <summary>
        /// Пока все потоки не завершаться, то приложение не будет остановлено, при этом главный поток продолжит выполнение
        /// </summary>
        private void StartHandlerThreadsAsForeground()
        {
            StartHandlerThread(MessageQueueItemType.Email);
            StartHandlerThread(MessageQueueItemType.Sms);
            StartHandlerThread(MessageQueueItemType.Push);
        }

        /// <summary>
        /// Пока все потоки не завершаться, то приложение не будет остановлено
        /// </summary>
        private void StartHandlerThreadsAsBackground()
        {
            _queueHandlerThreads.Add(StartHandlerThread(MessageQueueItemType.Email, true));
            _queueHandlerThreads.Add(StartHandlerThread(MessageQueueItemType.Sms, true));
            _queueHandlerThreads.Add(StartHandlerThread(MessageQueueItemType.Push, true));

            _queueHandlerThreads.ForEach(x => x.Join());
        }

        /// <summary>
        /// Потоки завершаться, если завершиться главный поток
        /// </summary>
        private void StartHandlerThreadsAsBackgroundWithoutWaiting()
        {
            StartHandlerThread(MessageQueueItemType.Email, true);
            StartHandlerThread(MessageQueueItemType.Sms, true);
            StartHandlerThread(MessageQueueItemType.Push, true);
        }

        private Thread StartHandlerThread(MessageQueueItemType type, bool isBackground = false)
        {
            var handler = QueueHandlerFactory.GetTypeQueueHandler(type);

            var thread = new Thread(handler.Handle)
            {
                IsBackground = isBackground
            };

            /**/
            Console.WriteLine($"Started for type: {type} with thread Id {thread.ManagedThreadId}, IsBackground: {thread.IsBackground}, ThreadPool: {thread.IsThreadPoolThread}, Priority: {thread.Priority}, State: {thread.ThreadState}!");

            thread.Start();

            return thread;
        }
    }
}