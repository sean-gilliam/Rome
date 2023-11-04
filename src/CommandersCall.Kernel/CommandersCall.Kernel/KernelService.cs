namespace CommandersCall.Kernel
{
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Options;
	using Serilog;
	using CommandersCall.Kernel.Shared.Timers;

	public class KernelService : BackgroundService
	{
		private readonly ILogger _logger;
		private readonly Settings _settings;

		public KernelService(IOptions<Settings> settings, ILogger logger)
		{
			_settings = settings.Value;
			_logger = logger;
		}

		protected async override Task ExecuteAsync(CancellationToken cancellingToken)
		{
			_logger.Information("Starting game timer...");
			var timer = new GameTimer(_settings.gameTickInterval);
			//var ticker = timer.Tick.Publish();
			var ticker = timer.Tick.Subscribe(new GameTicker(_logger));

			while (!cancellingToken.IsCancellationRequested)
			{
				await Task.Delay(Timeout.InfiniteTimeSpan, cancellingToken);
			}

			ticker.Dispose();
			//TODO: develop more once tasks are implmemented
			// var tasks = AppDomain.CurrentDomain
			// 	.GetAssemblies()
			// 	.SelectMany(x => x.GetTypes())
			// 	.Where(x => typeof(ITask).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
			// 	.Select(x => Activator.CreateInstance(x) as ITask)
			// 	.ToList();

			// _logger.Information("Spinning up server related tasks...");
			// foreach (var task in tasks)
			// {
			// 	if (task == null)
			// 		continue;

			// 	Task.Run(() => task.Execute());
			// }

			// _logger.Information("Spinning down server related tasks...");
			// foreach (var task in tasks)
			// {
			// 	if (task == null)
			// 		continue;

			// 	task.Token.Cancel();
			// }
		}
	}
}
