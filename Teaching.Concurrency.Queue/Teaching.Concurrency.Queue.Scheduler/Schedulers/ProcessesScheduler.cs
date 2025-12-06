using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Teaching.Concurrency.Queue.DataAccess;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers 
{ 
    public class ProcessesScheduler
        : IScheduler
    {
        private const string HandlerProcessFileName = "Otus.Teaching.Concurrency.Queue.Handler.Process.exe";
        private const string HandlerProcessDirectory = @"..\Otus.Teaching.Concurrency.Queue.Handler.Process\bin\Debug\netcoreapp3.1";
        readonly List<Process> _queueHandlerProcesses = new List<Process>();

        // Метод для обработки очереди с использованием процессов.
        // Метод находится в классе ProcessesScheduler.
        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();
            
            Console.WriteLine("Process scheduler...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();

            // Запускаем обработчики для каждого типа сообщений.
            _queueHandlerProcesses.Add(StartHandlerProcess(MessageQueueItemType.Email));
            _queueHandlerProcesses.Add(StartHandlerProcess(MessageQueueItemType.Sms));
            _queueHandlerProcesses.Add(StartHandlerProcess(MessageQueueItemType.Push));
            
            // Ожидаем завершения всех процессов.
            _queueHandlerProcesses.ForEach(x => x.WaitForExit());
            
            stopWatch.Stop();
            
            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }

        // Метод для запуска процесса-обработчика для заданного типа сообщений.
        // Метод находится в классе ProcessesScheduler.
        private Process StartHandlerProcess(MessageQueueItemType type)
        {
            var startInfo = new ProcessStartInfo()
            {
                ArgumentList = {type.ToString()},
                FileName = GetPathToHandlerProcess(),
            };
            
            var process = Process.Start(startInfo);

            return process;
        }

        // Метод для получения пути к исполняемому файлу процесса-обработчика.
        // Метод находится в классе ProcessesScheduler.
        private string GetPathToHandlerProcess()
        {
            return Path.Combine(HandlerProcessDirectory, HandlerProcessFileName);
        }
    }
}