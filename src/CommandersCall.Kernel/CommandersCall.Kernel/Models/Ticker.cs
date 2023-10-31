namespace CommandersCall.Kernel.Models
{
	using System;
	using System.Globalization;

	public class Ticker : IObserver<long>
	{
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
			Console.WriteLine("Current tick: " + value.ToString(CultureInfo.InvariantCulture));
		}
	}
}
