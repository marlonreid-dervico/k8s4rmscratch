using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Worker.Data;
using Worker.Entities;
using Worker.Messaging;
using Worker.Workers;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            try
            {
                var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();



                var services = new ServiceCollection()
                    .AddSingleton(loggerFactory)
                    .AddLogging()
                    .AddSingleton<IConfiguration>(config)
                    .AddTransient<IVoteData, MySqlVoteData>()
                    .AddTransient<IMessageQueue, MessageQueue>()
                    .AddSingleton<QueueWorker>()
                    .AddDbContext<VoteContext>(builder =>
                    {
                        builder.UseSqlServer(config.GetConnectionString("VoteData"));
                        builder.LogTo(Console.WriteLine);
                    });

                var provider = services.BuildServiceProvider();
                var worker = provider.GetRequiredService<QueueWorker>();
                worker.Start();
            }
            catch (Exception e)
            {
                loggerFactory.CreateLogger("start").Log(LogLevel.Error, e.Message);
                throw;
            }

        }
    }
}
