namespace CommandersCall.Kernel.Shared.Timers
{
	using System;
	using System.Reactive.Linq;

	public class GameTimer
	{
		public readonly IObservable<long> Tick;

		public GameTimer(int tickInterval)
		{
			Tick = Observable.Interval(TimeSpan.FromMilliseconds(tickInterval));
		}
	}
}
