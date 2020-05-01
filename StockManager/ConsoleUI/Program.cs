using ConsoleUI.CommandManager;
using ConsoleUI.FileOperations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockDataProvider.Startup;
using StockDataServices.DataServices;
using System.IO;
using System.Threading.Tasks;

namespace StockDataProvider
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            await serviceProvider.GetService<IApplication>().Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services.Configure<StockClientSettings>(configuration.GetSection("StockClient"));
            services.AddTransient<IStockClient, StockClient>();

            services.Configure<CsvOperationsSettings>(configuration.GetSection("CsvOperations"));
            services.AddTransient<IFileOperations, CsvOperations>();

            services.Configure<CommandManagerSettings>(configuration.GetSection("CommandManager"));
            services.AddTransient<ICommandManager, CommandManager>();

            services.AddSingleton<IApplication, Application>();
        }
    }
}
