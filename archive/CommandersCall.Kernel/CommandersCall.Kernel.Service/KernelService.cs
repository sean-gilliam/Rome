namespace CommandersCall.Kernel.Service
{
	using System.ServiceProcess;

	public class KernelService : ServiceBase
	{
		protected override void OnStart(string[] args)
		{
			var kernel = new Kernel();
			kernel.Execute();
		}

		protected override void OnStop()
		{
		}
	}
}
