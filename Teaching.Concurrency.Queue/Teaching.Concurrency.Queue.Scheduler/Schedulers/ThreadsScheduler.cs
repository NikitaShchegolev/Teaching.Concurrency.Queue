using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Teaching.Concurrency.Queue.Handler.QueueHandlers;
using Teaching.Concurrency.Queue.DataAccess;
using Teaching.Concurrency.Queue.Handler;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class ThreadsScheduler : IScheduler
    {
        private readonly List<Thread> _queueHandlerThreads = new List<Thread>();

        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();
            
            Console.WriteLine($"Thread scheduler in thread: {Thread.CurrentThread.ManagedThreadId}...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();
            
            _queueHandlerThreads.Add(StartHandlerThread(MessageQueueItemType.Email));
            _queueHandlerThreads.Add(StartHandlerThread(MessageQueueItemType.Sms));
            _queueHandlerThreads.Add(StartHandlerThread(MessageQueueItemType.Push));
            
            stopWatch.Stop();

            /**/Console.WriteLine($"Handled queue {stopWatch.Elapsed}...");
        }

        private Thread StartHandlerThread(MessageQueueItemType type)
        {
            var handler = QueueHandlerFactory.GetTypeQueueHandler(type);

            var thread = new Thread(handler.Handle);

            /**/Console.WriteLine($"Started for type: {type} with thread Id {thread.ManagedThreadId}, IsBackground: {thread.IsBackground}, ThreadPool: {thread.IsThreadPoolThread}, Priority: {thread.Priority}, State: {thread.ThreadState}!");
            
            thread.Start();

            return thread;
        }
    }
}