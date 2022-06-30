using System;
using IsolatedAzureFunctionAppDemo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        var env = Environment.GetEnvironmentVariable("IS_PRODUCTION");
        if (env == "false")
        {
            s.AddSingleton<IData, DataImpl>();
        }
        else
        {
            s.AddSingleton<IData, DataProduction>();
        }

    })
    .Build();

host.Run();