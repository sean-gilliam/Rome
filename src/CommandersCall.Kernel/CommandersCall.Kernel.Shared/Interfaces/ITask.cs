namespace CommandersCall.Kernel.Shared.Interfaces
{
	using System.Threading;
	using System.Threading.Tasks;

	public interface ITask
	{
		string Name { get; }
		CancellationTokenSource Token { get; }
		Task Execute();
	}
}
