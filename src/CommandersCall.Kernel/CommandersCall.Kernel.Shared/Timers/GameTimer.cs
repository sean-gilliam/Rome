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

			// Create a continuous list of zeros to be emitted every _minTickValue (in milliseconds). Attach a function to this
			// list that is called everytime _tickInterval (in milliseconds) is updated. This function creates a continuous list
			// of longs to be emitted every _tickInterval (in milliseconds). Next we get a handle to the current inner generated
			// list. Subscribers to the outer list will get notified only when the inner list is updated (either when the inner list
			// emits a new value or a when a new inner list is created).
			//
			// We do this tomfoolery because observables are immutable by default. So everytime the underlying source of the obserable changes
			// a new observable is created and thus looses all its subscribers. Below is a way to circumvent that behaviour by creating a fascade
			// over the inner workings.
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
