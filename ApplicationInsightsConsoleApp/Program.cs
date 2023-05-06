namespace ApplicationInsightsConsoleApp
{
    #region using

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    #endregion

    public interface IMainRunner
    {
        public Task<int> RunAsync();
    }

    public class MainRunner : IMainRunner
    {
        private readonly ILogger<MainRunner> _logger;

        public MainRunner(ILogger<MainRunner> logger)
        {
            _logger = logger;
        }

        public async Task<int> RunAsync()
        {
            await Task.Run(() =>
            {
                _logger.LogInformation("Hello from Main Runner!");
            });

            return 0;
        }
    }

    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            var host = hostBuilder.Build();

            var taskResult = await host.Services.GetRequiredService<IMainRunner>().RunAsync();

            host.Dispose();

            return taskResult;
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    SetServices(services);
                })
                .ConfigureAppConfiguration( builder =>
                {
                    builder.AddConfiguration(configuration);
                });
        }

        private static void SetServices(IServiceCollection services)
        {
            services.AddTransient<IMainRunner, MainRunner>();
        }
    }
}