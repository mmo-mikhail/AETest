using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AETest.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                    logging.AddConsole();
                    logging.AddDebug(); // On Linux, this provider writes logs to /var/log/message

                    // Filter out massive amount of Microsoft
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                })
                .UseStartup<Startup>();
    }
}
