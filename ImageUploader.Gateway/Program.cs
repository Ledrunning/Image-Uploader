using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImageUploader.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddFile("Logs/image_Uploader-{Date}.txt")
                .AddEventSourceLogger()
                .AddDebug();

            var configuration = GetConfiguration();

            var s = configuration.GetSection("").Get<>();

            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                logger.LogInformation("Service started successfully.");
                BuildWebHost(args).Run();
            }
            catch (Exception e)
            {
                logger.LogInformation("An error occurred when the service was running: {e}", e);
            }
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();
            return configuration;
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}