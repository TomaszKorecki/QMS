using System;
using System.Collections.Generic;
using System.Linq;
using QmsLib.Models;

namespace QMS.Models {
	public class ServerQueue {
		public string Name { get; set; }
		public int Id { get; set; }
		public string ConnectionId { get; set; }
		public bool IsOpened { get; set; }

		public double MeanWaitingTime  {
			get { return WaitingHistory.Count == 0 ? 0 : WaitingHistory.Sum(x => (x.StopWaitingTime - x.StartWaitingTime).TotalSeconds) / WaitingHistory.Count; }
		}

		public int WaitingPeoplesCount {
			get { return WaitingPeoples.Count; }
		}

		public List<Person> WaitingPeoples = new List<Person>();
		public List<WaitingPersonMeta> WaitingHistory = new List<WaitingPersonMeta>();

		public List<int> WaitingHistoryValues {
			get { return WaitingHistory.Select(x => (x.StopWaitingTime - x.StartWaitingTime).Seconds).ToList(); }
		} 
	}
}