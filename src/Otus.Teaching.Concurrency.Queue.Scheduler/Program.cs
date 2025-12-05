using System;
using System.Diagnostics;
using System.Linq;
using Teaching.Concurrency.Queue.Access;
using Teaching.Concurrency.Queue.Scheduler.Schedulers;

namespace Teaching.Concurrency.Queue.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация базы данных
            InitializeDatabase();

            // Получение типа планировщика из аргументов командной строки
            var schedulerType = GetSchedulerType(args);

            // Создание и запуск планировщика
            var scheduler = CreateScheduler(schedulerType);
            scheduler.ProcessQueue();
        }

        // Метод для инициализации базы данных
        private static void InitializeDatabase()
        {
            using var dataContext = new DataContext();
            var dbInitializer = new DbInitializer(dataContext);
            dbInitializer.Initialize();
        }

        // Метод для получения типа планировщика из аргументов командной строки
        private static string GetSchedulerType(string[] args)
        {
            if (args == null || !args.Any())
                return "simple"; // По умолчанию используем простой планировщик

            return args[0].ToLower();
        }

        // Метод для создания планировщика по типу
        private static IScheduler CreateScheduler(string schedulerType)
        {
            return schedulerType switch
            {
                "simple" => new SimpleScheduler(),
                "threads" => new ThreadsScheduler(),
                "threadpool" => new ThreadPoolScheduler(),
                "tasks" => new TasksScheduler(),
                "processes" => new ProcessesScheduler(),
                "background" => new ThreadsBackgroundForegroundScheduler(),
                _ => new SimpleScheduler() // По умолчанию используем простой планировщик
            };
        }
    }
}