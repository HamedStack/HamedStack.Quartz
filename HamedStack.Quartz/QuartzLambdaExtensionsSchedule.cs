
// ReSharper disable All 
namespace Quartz
{
    public static partial class QuartzLambdaExtensions
    {
        public static Task<DateTimeOffset> ScheduleJob(this IScheduler scheduler, Action action, TimeSpan delay, TimeSpan interval, bool disallowConcurrentJob = false)
        {
            IJobDetail jobDetail;
            if (disallowConcurrentJob)
            {
                var data = new JobDataMap { { "DisallowConcurrentJobAction", action } };
                jobDetail = JobBuilder
                    .Create<DisallowConcurrentJob>()
                    .UsingJobData(data)
                    .Build();
            }
            else
            {
                var data = new JobDataMap { { "JobAction", action } };
                jobDetail = JobBuilder
                    .Create<Job>()
                    .UsingJobData(data)
                    .Build();
            }

            var trigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.UtcNow.Add(delay))
                .WithSimpleSchedule(s => s.WithInterval(interval).RepeatForever())
                .Build();

            return scheduler.ScheduleJob(jobDetail, trigger);
        }
        public static Task<DateTimeOffset> ScheduleJob(this IScheduler scheduler, Action action, int delay, int interval, bool disallowConcurrentJob = false) =>
                    ScheduleJob(scheduler, action, new TimeSpan(0, 0, 0, delay), new TimeSpan(0, 0, 0, interval), disallowConcurrentJob);
        public static Task<DateTimeOffset> ScheduleJob(this IScheduler scheduler, Action action, Func<TriggerBuilder, TriggerBuilder> triggerBuilder, bool disallowConcurrentJob = false)
        {
            IJobDetail jobDetail;
            if (disallowConcurrentJob)
            {
                var data = new JobDataMap { { "DisallowConcurrentJobAction", action } };
                jobDetail = JobBuilder
                    .Create<DisallowConcurrentJob>()
                    .UsingJobData(data)
                    .Build();
            }
            else
            {
                var data = new JobDataMap { { "JobAction", action } };
                jobDetail = JobBuilder
                    .Create<Job>()
                    .UsingJobData(data)
                    .Build();
            }

            var trigger = triggerBuilder(TriggerBuilder.Create()).Build();

            return scheduler.ScheduleJob(jobDetail, trigger);
        }
        public static Task<DateTimeOffset> ScheduleJob(this IScheduler scheduler, Action action, string cron, bool disallowConcurrentJob = false)
        {
            IJobDetail jobDetail;
            if (disallowConcurrentJob)
            {
                var data = new JobDataMap { { "DisallowConcurrentJobAction", action } };
                jobDetail = JobBuilder
                    .Create<DisallowConcurrentJob>()
                    .UsingJobData(data)
                    .Build();
            }
            else
            {
                var data = new JobDataMap { { "JobAction", action } };
                jobDetail = JobBuilder
                    .Create<Job>()
                    .UsingJobData(data)
                    .Build();
            }

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule(cron)
                .Build();

            return scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
