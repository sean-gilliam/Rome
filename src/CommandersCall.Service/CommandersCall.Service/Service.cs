namespace CommandersCall.Service
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using Serilog.Events;
	using Serilog.Formatting.Json;
	using Serilog.Sinks.SystemConsole.Themes;
    using CommandersCall.Kernel;

	public class Service
	{
		public async Task ExecuteAsync(string[] args)
		{
			var logger = GetLogger();
			logger.Information("Starting Commander's Call...");

			var host = Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(config => config
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
				.UseSerilog(logger)
				.ConfigureServices((context, services) => 
				{
					services.Configure<Settings>(context.Configuration.GetSection("Settings"));
					services.AddHostedService<KernelService>();
				})
				.UseConsoleLifetime()
				.Build();

			string bug = "oops";
			logger.Debug("Some bug occurred {bug}", bug);

			await host.RunAsync();

			try
			{
				await host.WaitForShutdownAsync();
			}catch{}
		}

		private ILogger GetLogger()
		{
			return new LoggerConfiguration()
				.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug, theme: AnsiConsoleTheme.Code)
				.WriteTo.Logger(config => config.Filter
					.ByIncludingOnly(evt => evt.Level == LogEventLevel.Debug)
					.WriteTo.File(
						formatter: new JsonFormatter(),
						path: "bugs.log"))
				.WriteTo.File(
					formatter: new JsonFormatter(),
					path: "commanders.log",
					restrictedToMinimumLevel: LogEventLevel.Information, 
					rollingInterval: RollingInterval.Day)
				.MinimumLevel.Debug()
				.CreateLogger();
		}
	}
}
