using ActiveSessionMonitor;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ActiveSessionMonitoring>();
    })
    .Build();

host.Run();
