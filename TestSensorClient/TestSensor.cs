using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QmsLib.Models;

namespace TestSensorClient {
	class TestSensor : Sensor {

		public override string Name {
			get { return "TestSensor"; }
		}

		public override object MeasuredData {
			get { return 12345; }
		}
	}
}
