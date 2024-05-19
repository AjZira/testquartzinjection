using Quartz;

public class MyJob : IJob
{
    private readonly IService _Service;

    public MyJob(IService myService)
    {
        _Service = myService;
    }
    public Task Execute(IJobExecutionContext context)
    {
        // Code that sends a periodic email to the user (for example)
        // Note: This method must always return a value 
        // This is especially important for trigger listeners watching job execution 
        Console.Out.WriteLine("Hello");
        return Task.CompletedTask;
    }
}