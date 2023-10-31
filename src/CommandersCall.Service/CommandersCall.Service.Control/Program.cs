namespace CommandersCall.Kernel.Control
{
	using System.IO.Pipes;
	using System.Text;
	using CommandersCall.Configuration;

	public class Program
	{
		public static void Main(string[] args)
		{
			var server = Configuration.Server;
			var pipe = Configuration.PipeName;

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
