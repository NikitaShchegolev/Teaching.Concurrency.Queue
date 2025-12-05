using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Teaching.Concurrency.Queue.Access;

namespace Teaching.Concurrency.Queue.Handler.Process
{
    class Program
    {
        private static MessageQueueItemType _messageType;
        private static bool _isGenerateDb;

        static void Main(string[] args)
        {
            // Обработка аргументов командной строки.
            HandleArgs(args);

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            Console.WriteLine($"Handling queue for type: {_messageType}...");

            // Получаем обработчик очереди для заданного типа сообщений.
            var queueHandler = QueueHandlerFactory.GetTypeQueueHandler(_messageType);

            // Запускаем обработку очереди.
            queueHandler.Handle();

            stopWatch.Stop();

            Console.WriteLine($"Handled queue for type {_messageType} in {stopWatch.Elapsed}...");
        }


        // Метод для обработки аргументов командной строки.
        // Метод находится в классе Program.
        private static void HandleArgs(string[] args)
        {
            if(args == null || !args.Any())
                throw new ArgumentException("Required command line arguments");

            string typeStr = "";
            string generateDbStr = "";
            if (args.Length == 1)
            {
                typeStr = args[0];   
            }
            else if (args.Length == 2)
            {
                typeStr = args[0];
                generateDbStr = args[1];
            }
            else
            {
                throw new ArgumentException("Required right command line arguments");
            }
            
            // Проверяем и устанавливаем тип сообщения.
            if(!MessageQueueItemType.TryParse(typeStr, true, out _messageType))
                throw new ArgumentException("Not valid message type");
            
            Console.WriteLine($"Started queue handler for type: {_messageType} with process Id {System.Diagnostics.Process.GetCurrentProcess().Id}!");

            // Проверяем, нужно ли сгенерировать базу данных.
            if (!string.IsNullOrWhiteSpace(generateDbStr))
            {
                if(bool.TryParse(generateDbStr, out _isGenerateDb))
                {
                    if (_isGenerateDb)
                    {
                        using var dataContext = new DataContext();
                        new DbInitializer(dataContext).Initialize();           
                    }
                }
            }
        }
    }
}