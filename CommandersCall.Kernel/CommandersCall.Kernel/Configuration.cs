namespace CommandersCall.Kernel
{
	using System.Configuration;

	public static class Configuration
	{
		private static uint _tickerInterval;
		public static uint TickerInterval
		{
			get
			{
				if (_tickerInterval == default(uint))
				{
					uint value;
					if (!uint.TryParse(ConfigurationManager.AppSettings["game.tickInterval"], out value))
						value = 5000;

					_tickerInterval = value;
					return _tickerInterval;
				}

				return _tickerInterval;

			}
			set { _tickerInterval = value; }
		}

		private static string _pipeName;
		public static string PipeName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_pipeName))
					_pipeName = ConfigurationManager.AppSettings["config.pipe"];

				return _pipeName;
			}
		}
	}
}
