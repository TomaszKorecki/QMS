using System;
using System.Collections.Generic;
using System.Linq;
using QmsLib.Models;
using QMS.Models;

namespace QMS.Services {
	public class QueuesRepository {
		private static QueuesRepository instance;

		private QueuesRepository() {
			Queues = new Dictionary<int, ServerQueue>();
		}

		public static QueuesRepository Instance {
			get {
				if (instance == null)
					instance = new QueuesRepository();
				return instance;
			}
		}

		public Dictionary<int, ServerQueue> Queues { get; set; }

		public void ResetQueue(string connectionId) {
			var queue = Queues.FirstOrDefault(x => x.Value.ConnectionId == connectionId);
			Queues.Remove(queue.Key);
		}

		public ServerQueue ChooseQueueForPerson(Person person) {
			var queues = Queues.Values.ToList().Where(x => x.IsOpened).OrderBy(x => x.Id).ToList();

			var firstQueue = queues.First();

			if (person.IsCrippled || person.IsPregnant) {
				var mostShorterQueue = queues.First(x => x.WaitingPeoplesCount == queues.Min(y => y.WaitingPeoplesCount));
				mostShorterQueue.WaitingPeoples.Add(person);
				return mostShorterQueue;
			}

			if (queues.Skip(1).All(x => x.WaitingPeoplesCount > firstQueue.WaitingPeoplesCount) && firstQueue.WaitingPeoplesCount <= 1) {
				firstQueue.WaitingPeoples.Add(person);
				return firstQueue;	
			}

			//Let's always have first queue missed
			var nextQueues = queues.Skip(1).OrderBy(x => x.WaitingPeoples.Count);

			var next = nextQueues.First();
			next.WaitingPeoples.Add(person);
			return next;
		}

		public void RemovePersonFromQueue(WaitingPersonMeta personMeta, int queueId) {
			if (Queues.ContainsKey(queueId)) {
				var queue = Queues[queueId];

				queue.WaitingHistory.Add(personMeta);

				if (queue.WaitingPeoplesCount > 0)
					queue.WaitingPeoples.RemoveAll(x => x.Guid == personMeta.PersonGuid);
			}
		}

		public void StartQueue(int id) {
			var queue = Queues[id];

			if (queue != null) {
				queue.IsOpened = true;
			}
		}

		public void StopQueue(int id) {
			var queue = Queues[id];

			if (queue != null) {
				queue.IsOpened = false;
			}
		}
	}
}
