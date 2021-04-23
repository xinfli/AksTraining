using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appSettings.json");

            var configuration = configBuilder.Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, svcCollection) =>
                {

                    if (configuration != null)
                    {
                        svcCollection.Configure<MessageClientOptions>(configuration.GetSection("MessageClientOptions"));
                    }

                    svcCollection.AddHttpClient();
                    svcCollection.AddTransient<AksMessageSender>();
                }).UseConsoleLifetime();

            var host = builder.Build();

            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;

            try
            {
                var messageSender = services.GetRequiredService<AksMessageSender>();

                var result = await messageSender.SendMessage();
                Console.WriteLine($"Done! Now there are {result} message in target server");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occurred");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}
