using System;
using System.Diagnostics;
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
            ILogger logger = null;

            try
            {
                var loggerFactory = CreateLogger();

                logger = loggerFactory.CreateLogger<Program>();

                logger.LogInformation("Service started successfully.");
                BuildWebHost(args).Run();
            }
            catch (Exception e)
            {
                Trace.Write(
                    $"[{DateTime.Now:HH:mm:ss.fff}] Application startup error! Details {e.Message}");
                logger.LogInformation("An error occurred when the service was running: {e}", e);
            }
        }

        private static ILoggerFactory CreateLogger()
        {
            var configuration = GetConfiguration();
            var binPath = AppContext.BaseDirectory;
            var filePath = configuration.GetSection("Logging")
                .GetSection("File")["FilePath"];

            var loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddFile(Path.Combine(binPath, filePath))
                .AddEventSourceLogger()
                .AddDebug();
            return loggerFactory;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
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