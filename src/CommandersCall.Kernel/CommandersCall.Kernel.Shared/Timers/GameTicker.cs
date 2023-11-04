namespace CommandersCall.Kernel.Shared.Timers
{
	using System;
	using System.Globalization;
	using Serilog;

	public class GameTicker : IObserver<long>
	{
		private readonly ILogger _logger;

		public GameTicker(ILogger logger)
		{
			_logger = logger;
		}
		public void OnCompleted()
		{
			//here to satisfy contract
		}

		public void OnError(Exception error)
		{
			//here to satisfy contract
		}

		public void OnNext(long value)
		{
			_logger.Information("Current tick: " + value.ToString(CultureInfo.InvariantCulture));
		}
	}
}
