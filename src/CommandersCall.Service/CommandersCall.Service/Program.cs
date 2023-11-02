namespace CommandersCall.Service
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var service = new Service();
			await service.ExecuteAsync(args);
		}
	}
}
