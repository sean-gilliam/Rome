namespace CommandersCall.Kernel.Tasks
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Serilog;

    public class TaskManager
	{
		private readonly IDictionary<ITask, CancellationToken> _tasks;
		private readonly object _lock;

		private readonly ILogger _logger;
		private readonly Settings _settings;

		public TaskManager(IOptions<Settings> settings, ILogger logger)
		{
			_settings = settings.Value;
			_logger = logger;
			_tasks = new Dictionary<ITask, CancellationToken>();
			_lock = new object();
		}

		public void Queue(ITask task, CancellationToken token)
		{
			lock (_lock)
			{
				if(!_tasks.ContainsKey(task))
					_tasks.Add(task, token);
			}
		}

		public void Dequeue(ITask task)
		{
			lock (_lock)
			{
				if (_tasks.ContainsKey(task))
					_tasks.Remove(task);
			}
		}

		public void Execute()
		{
			var tasks = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => typeof(ITask).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(x => Activator.CreateInstance(x) as ITask)
				.ToList();

			Task? gt = null;
			foreach (var t in tasks)
			{
				if(t == null)
					continue;

				var token = new CancellationToken();
				_tasks.Add(t, token);

				var task = t.Execute(token);
				if (t.GetType() == typeof(ClockTask))
					gt = task;
			}

			if (gt != null)
				gt.Wait();
		}
	}
}
