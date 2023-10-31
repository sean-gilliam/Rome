namespace CommandersCall.Kernel
{
	using Microsoft.Extensions.Configuration;

	public static class Configuration
	{
		private static IConfiguration _configuration;

		static Configuration()
		{
			_configuration = new ConfigurationBuilder()
				.SetBasePath(".")
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				//.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables()
				.Build();
		}
		private static uint _tickerInterval;
		public static uint TickerInterval
		{
			get
			{
				if (_tickerInterval == default(uint))
				{
					uint value;
					if (!uint.TryParse(_configuration.GetValue<string>("Settings.game.tickInterval"), out value))
						value = 5000;

					_tickerInterval = value;
					return _tickerInterval;
				}

				return _tickerInterval;

			}
			set { _tickerInterval = value; }
		}

		private static string? _pipeName;
		public static string PipeName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_pipeName))
					_pipeName = _configuration.GetValue<string>("Settings.config.pipe");

				return _pipeName;
			}
		}
	}
}
