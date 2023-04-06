using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LE.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, builder) =>
                {
                    BuildConfiguration(builder, builderContext);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void BuildConfiguration(IConfigurationBuilder builder, HostBuilderContext context)
        {
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            builder.AddJsonFile($"appsettings.local.json", optional: true, reloadOnChange: true);
            builder.AddJsonFile($"message-channel.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
        }
    }
}
