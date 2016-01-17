using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QmsLib.Models {
	public class Person {

		public Guid Guid { get; set; }
		public double Weight { get; set; }
		public bool IsPregnant { get; set; }
		public double Temperature { get; set; }
		public bool IsInvalid { get; set; }

		//public override String ToString() {
		//	return Name + ": " + (IsPregnant ? "Pregnant" : "Not pregnant") + ", " + (IsInvalid ? "Invalid" : "Not invalid") +
		//			", " + Age + " years, " + (int)Weight + " kg, " + (int)Temperature + " st. C";
		//}
	}
}
