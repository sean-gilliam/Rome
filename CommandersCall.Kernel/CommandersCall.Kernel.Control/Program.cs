namespace CommandersCall.Kernel.Control
{
	using System.Configuration;
	using System.IO.Pipes;
	using System.Text;

	public class Program
	{
		public static void Main(string[] args)
		{
			var server = ConfigurationManager.AppSettings["config.server"];
			var pipe = ConfigurationManager.AppSettings["config.pipe"];

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
