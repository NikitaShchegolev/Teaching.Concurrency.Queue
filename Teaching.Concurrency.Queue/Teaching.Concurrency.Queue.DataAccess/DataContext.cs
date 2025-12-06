using Microsoft.EntityFrameworkCore;

namespace Teaching.Concurrency.Queue.DataAccess
{
    public class DataContext : DbContext
    {
        // Коллекция сообщений в очереди. Поле находится в классе DataContext.
        public DbSet<MessageQueueItem> MessageQueue { get; set; }

        // Метод для настройки параметров подключения к базе данных.
        // Метод находится в классе DataContext.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Устанавливаем использование SQLite с указанным файлом базы данных.
            optionsBuilder.UseSqlite("Filename=OtusTeachingConcurrency.sqlite");
        }
    }
}