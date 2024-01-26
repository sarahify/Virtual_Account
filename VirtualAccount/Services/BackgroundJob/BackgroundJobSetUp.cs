using Microsoft.Extensions.Options;
using Quartz;

namespace VirtualAccount.Services.BackgroundJob
{
    public class BackgroundJobSetUp : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(VirtualAccountCreationClass));
            options
            .AddJob<VirtualAccountCreationClass>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
            trigger
            .ForJob(jobKey)
            .WithSimpleSchedule(schedule =>
            schedule.WithIntervalInMinutes(30).RepeatForever()));
        }
    }
}
