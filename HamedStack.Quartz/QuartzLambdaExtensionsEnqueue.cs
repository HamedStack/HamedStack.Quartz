
// ReSharper disable All 
namespace Quartz
{
    public static partial class QuartzLambdaExtensions
    {
        public static Task<DateTimeOffset> Enqueue(this IScheduler scheduler, Action action, bool disallowConcurrentJob = false)
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
                .Build();

            return scheduler.ScheduleJob(jobDetail, trigger);
        }
        public static Task<DateTimeOffset> Enqueue<T>(this IScheduler scheduler, Action<T> action, bool disallowConcurrentJob = false)
                    where T : new()
        {
            IJobDetail jobDetail;
            if (disallowConcurrentJob)
            {
                var data = new JobDataMap { { "DisallowConcurrentJobWithTypeAction", action.Convert()! }, { "DisallowConcurrentJobWithType", new T() } };
                jobDetail = JobBuilder
                    .Create<DisallowConcurrentJobWithType>()
                    .UsingJobData(data)
                    .Build();
            }
            else
            {
                var data = new JobDataMap { { "JobWithTypeAction", action.Convert()! }, { "JobWithType", new T() } };
                jobDetail = JobBuilder
                    .Create<JobWithType>()
                    .UsingJobData(data)
                    .Build();
            }

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .Build();

            return scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
