using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QmsLib.Models;

namespace QMS.Models {
	public class HandledPerson {
		public Person Person { get; set; }
		public int QueueId { get; set; }
	}
}