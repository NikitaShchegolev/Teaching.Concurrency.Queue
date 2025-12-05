namespace Teaching.Concurrency.Queue.Otus.Teaching.Concurrency.Queue.Scheduler
{
    internal class EventingBasicConsumer
    {
        private object channel;

        public EventingBasicConsumer(object channel)
        {
            this.channel = channel;
        }

        public System.Action<object, object> Received { get; internal set; }
    }
}