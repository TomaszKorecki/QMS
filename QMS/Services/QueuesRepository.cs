using System;
using System.Collections.Generic;
using System.Linq;
using QmsLib.Models;
using QMS.Models;

namespace QMS.Services {
	public class QueuesRepository {
		private static QueuesRepository instance;

		private QueuesRepository() {
			Queues = new List<ServerQueue>();
		}

		public static QueuesRepository Instance {
			get {
				if (instance == null)
					instance = new QueuesRepository();
				return instance;
			}
		}

		public List<ServerQueue> Queues { get; set; }

		public void ResetQueue(string connectionId) {
			Queues.RemoveAll(x => x.ConnectionId == connectionId);
		}

		public ServerQueue ChooseQueueForPerson(Person person) {
			var firstQueue = Queues.FirstOrDefault(x => x.Id == 1);
			var secondQueue = Queues.FirstOrDefault(x => x.Id == 2);
			var thirdQueue = Queues.FirstOrDefault(x => x.Id == 3);

			if (person.IsInvalid || person.IsPregnant && (firstQueue.WaitingPeoplesCount < secondQueue.WaitingPeoplesCount &&
														firstQueue.WaitingPeoplesCount < thirdQueue.WaitingPeoplesCount)) {

				firstQueue.WaitingPeoples.Enqueue(person);
				return firstQueue;
			}


			if (secondQueue.WaitingPeoplesCount > thirdQueue.WaitingPeoplesCount) {
				thirdQueue.WaitingPeoples.Enqueue(person);
				return thirdQueue;
			}

			secondQueue.WaitingPeoples.Enqueue(person);
			return secondQueue;
		}

		public void RemovePersonFromQueue(Guid guid, int queueId) {
			var queue = Queues.FirstOrDefault(x => x.Id == queueId);
			queue.PeopleHandled++;
			queue.WaitingPeoples.Dequeue();
		}
	}
}
