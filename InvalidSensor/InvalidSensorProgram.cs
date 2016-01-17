using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QmsLib;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace InvalidSensor {
	class InvalidSensorProgram {
		static void Main(string[] args) {
			ISensor sensor = new InvalidSensor();
			Console.WriteLine("Invalid sensor init");
			sensor.ConnectToServer();
			Console.WriteLine("Invalid sensor connected");
			Console.ReadLine();
			sensor.DisconnectFromServer();
		}
	}

	class InvalidSensor : Sensor {

		private const double INVALID_COEFFICIENT = 0.16d;

		public override string Name {
			get { return "InvalidSensor"; }
		}

		public override object MeasuredData {
			get {
				return RandomUtils.NextDouble() < INVALID_COEFFICIENT;
			}
		}
	}
}
