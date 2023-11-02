namespace CommandersCall.Kernel.Tasks
{
	// using System;
	// using System.IO;
	// using System.IO.Pipes;
	// using System.Security.AccessControl;
	// using System.Security.Principal;
	// using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Options;
	using Serilog;

	public class ConfigurationTask : ITask
	{
		private readonly ILogger _logger;
		private readonly Settings _settings;

		public ConfigurationTask(IOptions<Settings> settings, ILogger logger)
		{
			_settings = settings.Value;
			_logger = logger;
		}

		public Task Execute(CancellationToken token)
		{
			// return Task.Factory.StartNew(() =>
			// {
			// 	var pipeSecurity = new PipeSecurity();
			// 	pipeSecurity.AddAccessRule(
			// 			new PipeAccessRule(WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, AccessControlType.Allow)
			// 	);
			// 	pipeSecurity.AddAccessRule(
			// 			new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.ReadWrite, AccessControlType.Allow)
			// 	);

			// 	while (true)
			// 	{
			// 		if (token.IsCancellationRequested)
			// 			break;

			// 		using (var s = new NamedPipeServerStream(Configuration.PipeName, PipeDirection.InOut, -1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
			// 		{
			// 			s.WaitForConnection();

			// 			var command = Encoding.UTF8.GetString(ReadMessage(s));
			// 			Console.WriteLine("[Server] " + command);
			// 		}
			// 	}
			// }, token, TaskCreationOptions.None, PriorityScheduler.Lowest);
			return Task.CompletedTask;
		}

		// private static byte[] ReadMessage(PipeStream s)
		// {
		// 	var ms = new MemoryStream();
		// 	var buffer = new byte[0x1000];      // Read in 4KB blocks

		// 	do
		// 	{
		// 		ms.Write(buffer, 0, s.Read(buffer, 0, buffer.Length));
		// 	}
		// 	while (!s.IsMessageComplete);

		// 	return ms.ToArray();
		// }
	}
}
