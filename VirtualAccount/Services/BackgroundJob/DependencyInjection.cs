using Quartz;

namespace VirtualAccount.Services.BackgroundJob
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            // what happens to the background job when application is running down
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<BackgroundJobSetUp>();
        }
    }
}
