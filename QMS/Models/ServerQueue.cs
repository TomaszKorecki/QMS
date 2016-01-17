using System;
using System.Collections.Generic;
using System.Linq;
using QmsLib.Models;

namespace QMS.Models {
	public class ServerQueue {
		public string Name { get; set; }
		public int Id { get; set; }
		public string ConnectionId { get; set; }
		public bool Working { get; set; }

		//public int PeopleHandled {
		//	get { return WaitingTimes.Count; }
		//}

		public int PeopleHandled { get; set; }

		public double MeanWaitingTime  {
			get { return WaitingTimes.Select(x => x.Item2).Sum(x => x.TotalMilliseconds)/PeopleHandled; }
		}

		public int WaitingPeoplesCount {
			get { return WaitingPeoples.Count; }
		}

		public Queue<Person> WaitingPeoples = new Queue<Person>();
		public List<Tuple<DateTime, TimeSpan>> WaitingTimes = new List<Tuple<DateTime, TimeSpan>>();  
	}
}