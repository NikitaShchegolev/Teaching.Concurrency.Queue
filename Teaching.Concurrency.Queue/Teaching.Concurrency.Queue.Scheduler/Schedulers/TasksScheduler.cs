using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Teaching.Concurrency.Queue.DataAccess;
using Teaching.Concurrency.Queue.Handler;

namespace Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class TasksScheduler : IScheduler
    {
        // Список задач для обработки очереди
        private readonly List<TaskSchedulerItem> _queueHandlerTasks = new List<TaskSchedulerItem>();
        // Максимальное количество попыток повторного выполнения задачи
        private readonly int _retryCount =3;

        // Основной метод для обработки очереди
        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();
            Console.WriteLine("Task scheduler...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();

            // Запуск задач для обработки очереди
            StartAsTasks();

            stopWatch.Stop();
            
            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }

        // Метод для инициализации и запуска задач
        private void StartAsTasks()
        {
            // Создание задач для обработки различных типов сообщений
            var emailTask = new TaskSchedulerItem(MessageQueueItemType.Email);
            var smsTask = new TaskSchedulerItem(MessageQueueItemType.Sms);
            var pushTask = new TaskSchedulerItem(MessageQueueItemType.Push);
            
            // Добавление задач в список
            _queueHandlerTasks.Add(emailTask);
            _queueHandlerTasks.Add(smsTask);
            _queueHandlerTasks.Add(pushTask);
            
            // Запуск каждой задачи
            _queueHandlerTasks.ForEach(StartHandlerTask);
            
            // Ожидание завершения всех задач с учетом повторных попыток
            WaitAllTasksWithRetry();
        }

        // Метод для ожидания завершения всех задач с учетом повторных попыток
        private void WaitAllTasksWithRetry()
        {
            try
            {
                // Получение списка задач для ожидания
                var tasksForWait = GetTasksForWait();
                Task.WaitAll(tasksForWait);
            }
            catch (Exception)
            {
                // Повторный запуск задач, завершившихся с ошибкой
                RetryFaultTasks();
                WaitAllTasksWithRetry();
            }
        }

        // Метод для получения списка задач, которые еще не исчерпали лимит попыток
        private Task[] GetTasksForWait()
        {
            return _queueHandlerTasks
                .Where(x => x.RetryCount <= _retryCount)
                .Select(x => x.Task)
                .ToArray();
        }

        // Метод для повторного запуска задач, завершившихся с ошибкой
        private void RetryFaultTasks()
        {
            _queueHandlerTasks
                .Where(t => t.Task.IsFaulted)
                .ToList()
                .ForEach(RetryHandle);
        }

        // Метод для запуска обработчика задачи
        private void StartHandlerTask(TaskSchedulerItem item)
        {
            // Получение обработчика очереди для типа задачи
            var handler = QueueHandlerFactory.GetTypeQueueHandler(item.Type);
            // Асинхронный запуск задачи
            item.Task = Task.Run(() => handler.Handle());
            // Увеличение счетчика попыток выполнения задачи
            item.RetryCount +=1;
        }

        // Метод для повторного запуска задачи
        private void RetryHandle(TaskSchedulerItem item)
        {
            Console.WriteLine($"Пытаемся заново запустить обработчик {item.Type}, так как произошла ошибка: {item.Task?.Exception}");
            if (item.RetryCount <= _retryCount)
            {
                StartHandlerTask(item);
            }
        }
    }
}