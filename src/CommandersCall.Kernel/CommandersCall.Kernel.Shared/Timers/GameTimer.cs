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
			_tickInterval = Math.Clamp(tickInterval, _minTickValue, _maxTickValue);
		}

		public void Start()
		{
			_cancelToken = new CancellationTokenSource();

			_tick = Observable
			// 	.Interval(TimeSpan.FromMilliseconds(1000))
				.Generate(0L, i => !_cancelToken.IsCancellationRequested, i => i + 1, i => i, i => TimeSpan.FromMilliseconds(_minTickValue))
				.Window(() => Observable.Interval(TimeSpan.FromMilliseconds(_tickInterval)))
				.Select(x => x.LastAsync())
				.Switch();

			_subscriber = _tick.Subscribe(_ticker);
		}

		public void Stop()
		{
			_subscriber.Dispose();
			_cancelToken.Cancel();
		}
	}
}
