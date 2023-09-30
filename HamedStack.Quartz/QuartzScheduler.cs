// ReSharper disable UnusedMember.Global
// ReSharper disable once CheckNamespace

using Quartz.Impl;

// Calling methods in background
// BackgroundJob.Enqueue<IEmailSender>(x => x.Send("hangfire@example.com"));
// BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

// Calling methods with delay
// BackgroundJob.Schedule(() => Console.WriteLine("Hello, world"), TimeSpan.FromDays(1));

// Performing recurrent tasks
// RecurringJob.AddOrUpdate(() => Console.Write("Easy!"), Cron.Daily);
// RecurringJob.AddOrUpdate(() => Console.Write("Powerful!"), "0 12 * */2");
// RecurringJob.AddOrUpdate("some-id", () => Console.WriteLine(), Cron.Hourly);
// RecurringJob.RemoveIfExists("some-id");
// RecurringJob.Trigger("some-id");

// Using Batches
// BatchJob.StartNew(x =>
// {
//     for (var i = 0; i < 1000; i++)
//     {
//         x.Enqueue(() => SendEmail(i));
//     }
// });

// Same JobKey = same job.
// Different JobKey = different job.
// Quartz won't let you use the same JobKey more than once because that'd be two jobs with the same key. Like having two users with the same ID.
// DisallowConcurrentExecution avoids overlapping executions of the same job. If you use a different JobKey, it's not the same job anymore, so the annotation doesn't have any effect. 

// ReSharper disable All 
namespace Quartz
{
    public static class QuartzScheduler
    {
        private static readonly StdSchedulerFactory Factory = new StdSchedulerFactory();
        public static async Task Delay(Action action, TimeSpan delay, bool disallowConcurrentJob = false)
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.Delay(action, delay, disallowConcurrentJob);
        }
        public static async Task Delay<T>(Action<T> action, TimeSpan delay, bool disallowConcurrentJob = false) where T : new()
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.Delay(action, delay, disallowConcurrentJob);
        }
        public static async Task Enqueue(Action action, bool disallowConcurrentJob = false)
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.Enqueue(action, disallowConcurrentJob);
        }
        public static async Task Enqueue<T>(Action<T> action, bool disallowConcurrentJob = false) where T : new()
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.Enqueue(action, disallowConcurrentJob);
        }
        public static async Task Schedule(Action action, TimeSpan delay, TimeSpan interval, bool disallowConcurrentJob = false)
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.ScheduleJob(action, delay, interval, disallowConcurrentJob);
        }
        public static async Task Schedule(Action action, int delay, int interval, bool disallowConcurrentJob = false)
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.ScheduleJob(action, delay, interval, disallowConcurrentJob);
        }
        public static async Task Schedule(Action action, Func<TriggerBuilder, TriggerBuilder> triggerBuilder, bool disallowConcurrentJob = false)
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.ScheduleJob(action, triggerBuilder, disallowConcurrentJob);
        }
        public static async Task Schedule(Action action, string cron, bool disallowConcurrentJob = false)
        {
            var scheduler = await Factory.GetScheduler();
            await scheduler.Start();
            await scheduler.ScheduleJob(action, cron, disallowConcurrentJob);
        }
    }
}
