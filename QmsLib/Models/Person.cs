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
		public bool IsCrippled { get; set; }

		//public override String ToString() {
		//	return Name + ": " + (IsPregnant ? "Pregnant" : "Not pregnant") + ", " + (IsCrippled ? "Invalid" : "Not invalid") +
		//			", " + Age + " years, " + (int)Weight + " kg, " + (int)Temperature + " st. C";
		//}
	}
}
