namespace CommandersCall
{
	using Serilog;
	using Serilog.Events;
	using Serilog.Formatting.Json;

	public static class Logger
	{
		private static ILogger _logger;

		static Logger()
		{
			_logger = new LoggerConfiguration()
				.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
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

		public static ILogger Entry => _logger;
		
	}
}
