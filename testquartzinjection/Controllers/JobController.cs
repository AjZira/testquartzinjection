using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace testquartzinjection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IScheduler _scheduler;

        public JobController(ISchedulerFactory schedulerFactory)
        {
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create(string jobName, string? groupName, string? triggerName, string? cron)
        {
            IJobDetail job = JobBuilder.Create<MyJob>()
                .WithIdentity(jobName, groupName ?? "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerName, groupName ?? "group1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5)
                .RepeatForever())
                .StartNow()
                .Build();

            await _scheduler.ScheduleJob(job, trigger);

            return Ok(job.JobDataMap);
        }

        [HttpGet("Pause")]
        public async Task Pause(string jobName, string? groupName)
        {
            await _scheduler.PauseJob(new JobKey(jobName, groupName ?? "group1"));
        }

        [HttpGet("Resume")]
        public async Task Resume(string jobName, string? groupName)
        {
            await _scheduler.ResumeJob(new JobKey(jobName, groupName ?? "group1"));
        }
    }
}
