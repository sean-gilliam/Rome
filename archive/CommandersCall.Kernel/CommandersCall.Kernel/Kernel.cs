namespace CommandersCall.Kernel
{
	using CommandersCall.Kernel.Tasks;

	public class Kernel
	{
		public void Execute()
		{
			var tm = new TaskManager();
			tm.Execute();
		}
	}
}
