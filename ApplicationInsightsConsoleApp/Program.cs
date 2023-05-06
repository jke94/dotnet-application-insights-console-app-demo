namespace ApplicationInsightsConsoleApp
{
    #region using

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    #endregion

    public class Program
    {
        #region Fields

        private const string AppSettingsFileName = "appsettings.json";

        #endregion

        #region Main Method

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var hostBuilder = CreateHostBuilder(args);

                var host = hostBuilder.Build();

                var taskResult = await host.Services.GetRequiredService<IMainRunner>().RunAsync();

                host.Dispose();

                return taskResult;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Message: {e.Message}");
                Console.WriteLine($"Exception HResult: {e.HResult}");
                
                return -1;
            }
        }

        #endregion

        #region Private Methods

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    SetServices(hostContext, services);
                })
                .ConfigureAppConfiguration( (builder) =>
                {
                    builder.AddConfiguration(BuildingConfigurationBuilder(args));
                });
        }

        private static IConfigurationRoot BuildingConfigurationBuilder(string[] args)
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile(AppSettingsFileName)
                .Build();
        }

        private static void SetServices(
            HostBuilderContext hostBuilderContext, 
            IServiceCollection services)
        {
            services.Configure<TelemetryConfiguration>(config => config.TelemetryChannel = new InMemoryChannel());
            services.AddLogging(builder =>
            {
                builder.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => 
                    {
                        config.ConnectionString = hostBuilderContext
                        .Configuration["ApplicationInsights:InstrumentationKey"];
                    },
                    configureApplicationInsightsLoggerOptions: (options) => { }
                );
            });

            services.AddTransient<IMainRunner, MainRunner>();
        }

        #endregion
    }
}