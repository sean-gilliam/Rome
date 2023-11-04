namespace CommandersCall.Service
{
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using Serilog.Events;
	using Serilog.Filters;
	using Serilog.Formatting.Json;
	using Serilog.Sinks.SystemConsole.Themes;
	using CommandersCall.Kernel;

	public class Service
	{
		private readonly string _configDirectory;
		private readonly string _loggingDirectory;

		public Service()
		{
			var cwd = Directory.GetCurrentDirectory();
			_configDirectory = cwd;
			_loggingDirectory = Path.Combine(cwd, "logs");
		}

		public async Task ExecuteAsync(string[] args)
		{
			var logger = GetLogger();
			logger.Information("Starting Commander's Call...");

			var host = Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(config => config
					.SetBasePath(_configDirectory)
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
				.UseSerilog(logger)
				.ConfigureServices((context, services) =>
				{
					services.Configure<Settings>(context.Configuration.GetSection("Settings"));
					services.AddHostedService<KernelService>();
				})
				.UseConsoleLifetime()
				.Build();

			logger.Information("Commander's Call started. Press Ctrl+C or send SIGTERM to shut down.");
			await host.RunAsync();

			try
			{
				await host.WaitForShutdownAsync();
			}
			catch { }
		}

		private ILogger GetLogger()
		{
			var systemMatch = Matching.FromSource("Microsoft");

			return new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Logger(console => console.Filter
						.ByExcluding(systemMatch)
						.WriteTo.Console(
							restrictedToMinimumLevel: LogEventLevel.Information,
							theme: AnsiConsoleTheme.Code))
				.WriteTo.Logger(bugs => bugs.Filter
					.ByIncludingOnly(evt => evt.Level == LogEventLevel.Debug && !systemMatch(evt))
					.WriteTo.File(
						formatter: new JsonFormatter(),
						path: Path.Combine(_loggingDirectory, "bugs.log")))
				.WriteTo.Logger(trace => trace.Filter
					.ByIncludingOnly(systemMatch)
					.WriteTo.File(
						formatter: new JsonFormatter(),
						path: Path.Combine(_loggingDirectory, "trace.log")))
				.WriteTo.Logger(game => game.Filter
					.ByExcluding(systemMatch)
					.WriteTo.File(
						formatter: new JsonFormatter(),
						path: Path.Combine(_loggingDirectory, "commanders.log"),
						restrictedToMinimumLevel: LogEventLevel.Information,
						rollingInterval: RollingInterval.Day))
				.CreateLogger();
		}
	}
}
