namespace CommandersCall.Kernel.Tasks
{
	using System;
	using System.Reactive.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using CommandersCall.Kernel.Models;

	public class ClockTask : ITask
	{
		public readonly static IObservable<long> Tick = Observable.Interval(TimeSpan.FromMilliseconds(Configuration.TickerInterval));

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
