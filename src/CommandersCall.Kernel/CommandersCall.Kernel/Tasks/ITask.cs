namespace CommandersCall.Kernel.Tasks
{
	using System.Threading;
	using System.Threading.Tasks;

	public interface ITask
	{
		Task Execute(CancellationToken token);
	}
}
