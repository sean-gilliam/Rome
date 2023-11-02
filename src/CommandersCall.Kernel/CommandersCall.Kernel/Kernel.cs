namespace CommandersCall.Kernel
{
	using CommandersCall.Kernel.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Serilog;

    // public class Kernel
    // {
    // 	public void Execute()
    // 	{
    // 		var tm = new TaskManager();
    // 		tm.Execute();
    // 	}
    // }

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
			while(!cancellingToken.IsCancellationRequested) 
			{ 
				_logger.Debug("Working behind the scenes..."); 
				await Task.Delay(_settings.gameTickInterval, cancellingToken); 
			}
		} 
	}  
}
