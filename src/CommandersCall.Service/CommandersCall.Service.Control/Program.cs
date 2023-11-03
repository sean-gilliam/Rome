namespace CommandersCall.Kernel.Control
{
	using System.IO.Pipes;
	using System.Text;
	using CommandersCall;

	public class Program
	{
		public static void Main(string[] args)
		{
			// TODO: this obviosuly errors out. changes made only for compile reasons.
			// TODO: this whole project needs to be refactored. 
			var settings = new Settings();
			var server = settings.configServer;
			var pipe = settings.configPipe;

			using (var s = new NamedPipeClientStream(server, pipe, PipeDirection.InOut))
			{
				s.Connect();
				s.ReadMode = PipeTransmissionMode.Message;

				var command = Encoding.UTF8.GetBytes("tickInterval");
				s.Write(command, 0, command.Length);
			}
		}
	}
}
