namespace ApplicationInsightsConsoleApp
{
    #region using

    using Microsoft.Extensions.Logging;

    #endregion

    public interface IMainRunner
    {
        #region Methods

        public Task<int> RunAsync();

        #endregion
    }

    public class MainRunner : IMainRunner
    {
        #region Fields

        private const int NumberOfTraceLimit = 50;

        private readonly ILogger<MainRunner> _logger;
        private readonly Random _random;

        #endregion

        #region Constructor

        public MainRunner(ILogger<MainRunner> logger)
        {
            _logger = logger;
            _random = new Random();
        }

        #endregion

        #region Methods

        public async Task<int> RunAsync()
        {
            var traceNumber = 0;
            var waitingMillisecons = 0;

            await Task.Run(async () =>
            {
                while (traceNumber < NumberOfTraceLimit)
                {
                    waitingMillisecons = (_random.Next(1, 10)) * 1000;

                    _logger.LogInformation($"Hello from Main Runner! Trace {traceNumber} of {NumberOfTraceLimit} and waiting {waitingMillisecons} ms for the next trace.");

                    traceNumber++;

                    await Task.Delay(waitingMillisecons);
                }
            });

            return 0;
        }

        #endregion
    }
}
