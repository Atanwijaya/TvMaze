using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Interfaces;

namespace TVScapper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost webHost = CreateHostBuilder(args).Build();
            ITVMazeSetupService tVMazeSetupService = (ITVMazeSetupService)webHost.Services.GetService(typeof(ITVMazeSetupService));
            await tVMazeSetupService.SetupTVMazeArchitectureAsync();
            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
