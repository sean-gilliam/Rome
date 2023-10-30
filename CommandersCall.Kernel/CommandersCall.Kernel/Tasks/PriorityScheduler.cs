namespace CommandersCall.Kernel.Tasks
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	public class PriorityScheduler : TaskScheduler
	{
		public static PriorityScheduler Lowest = new PriorityScheduler(ThreadPriority.Lowest);
		public static PriorityScheduler BelowNormal = new PriorityScheduler(ThreadPriority.BelowNormal);
		public static PriorityScheduler Normal = new PriorityScheduler(ThreadPriority.Normal);
		public static PriorityScheduler AboveNormal = new PriorityScheduler(ThreadPriority.AboveNormal);
		public static PriorityScheduler Highest = new PriorityScheduler(ThreadPriority.Highest);

		private readonly BlockingCollection<Task> _tasks;
		private readonly ThreadPriority _priority;
		private Thread[] _threads;

		public override int MaximumConcurrencyLevel
		{
			get { return Environment.ProcessorCount; }
		}

		public PriorityScheduler(ThreadPriority priority)
		{
			_priority = priority;
			_tasks = new BlockingCollection<Task>();
		}

		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return _tasks;
		}

		protected override void QueueTask(Task task)
		{
			_tasks.Add(task);

			if (_threads != null)
				return;

			_threads = new Thread[MaximumConcurrencyLevel];
			for (var i = 0; i < _threads.Length; i++)
			{
				_threads[i] = new Thread(() =>
				{
					foreach (var t in _tasks.GetConsumingEnumerable())
						TryExecuteTask(t);
				});
				_threads[i].Name = string.Format("PriorityScheduler: ", i);
				_threads[i].Priority = _priority;
				_threads[i].IsBackground = true;
				_threads[i].Start();
			}
		}

		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			return false; // we might not want to execute task that should schedule as high or low priority inline
		}
	}
}
