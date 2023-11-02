namespace CommandersCall.Kernel.Tasks
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Serilog;
    using Microsoft.Extensions.Options;
	using CommandersCall.Kernel.Models;

    public class ClockTask : ITask
	{
		private readonly ILogger _logger;
		private readonly Settings _settings;

		public readonly IObservable<long> Tick;

		public ClockTask(IOptions<Settings> settings, ILogger logger)
		{
			_settings = settings.Value;
			_logger = logger;
			Tick = Observable.Interval(TimeSpan.FromMilliseconds(_settings.gameTickInterval));
		}

		public Task Execute(CancellationToken token)
		{
			return Task.Factory.StartNew(() =>
			{
				var sub = Tick.Subscribe(new Ticker());
				while (!token.IsCancellationRequested)
				{
				}

				sub.Dispose();
			}, token, TaskCreationOptions.None, PriorityScheduler.AboveNormal);
		}
	}
}
