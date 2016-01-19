using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QmsLib.Models {
	public class WaitingPersonMeta {
		public DateTime StartWaitingTime { get; set; }
		public DateTime StopWaitingTime { get; set; }
		public Guid PersonGuid { get; set; }
	}
}
