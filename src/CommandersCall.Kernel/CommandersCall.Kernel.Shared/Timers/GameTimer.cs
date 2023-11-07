namespace CommandersCall.Kernel.Shared.Timers
{
	using System;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using Serilog;

	public class GameTimer
	{
		private const int _minTickValue = 1000; // 1s
		private const int _maxTickValue = 60000; // 60s

		private readonly ILogger _logger;
		private int _tickInterval;
		private IObservable<long> _tick;
		private IDisposable _subscriber;
		private readonly GameTicker _ticker;
		private CancellationTokenSource _cancelToken;

		public IObservable<long> Tick => _tick;

		public GameTimer(ILogger logger, int tickInterval)
		{
			_logger = logger;
			_tickInterval = Math.Clamp(tickInterval, _minTickValue, _maxTickValue);

			_tick = Observable.Empty<long>();
			_subscriber = Disposable.Empty;
			_ticker = new GameTicker(_logger);
			_cancelToken = new CancellationTokenSource();
		}

		public void Update(int tickInterval)
		{
			_logger.Information("Updating game timer from [{_tickInterval}] to [{tickInterval}]...", _tickInterval, tickInterval);

			_tickInterval = Math.Clamp(tickInterval, _minTickValue, _maxTickValue);
		}

		public void Start()
		{
			_logger.Information("Starting game timer...");

			_cancelToken = new CancellationTokenSource();

			_tick = Observable
				.Generate(0L, i => !_cancelToken.IsCancellationRequested, i => 0L, i => i, i => TimeSpan.FromMilliseconds(_minTickValue))
				.Window(() => Observable.Interval(TimeSpan.FromMilliseconds(_tickInterval)))
				.Select(x => x.LastAsync())
				.Switch();

			_subscriber = _tick.Subscribe(_ticker);
		}

		public void Stop()
		{
			_logger.Information("Stopping game timer...");

			_subscriber.Dispose();
			_cancelToken.Cancel();
		}
	}
}
