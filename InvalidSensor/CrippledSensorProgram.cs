using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QmsLib;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace CrippledSensor {
	class CrippledSensorProgram {
		static void Main(string[] args) {
			ISensor sensor = new CrippledSensor();
			Console.WriteLine("Crippled sensor init");
			sensor.ConnectToServer();
			Console.WriteLine("Crippled sensor connected");
			Console.ReadLine();
			sensor.DisconnectFromServer();
		}
	}

	class CrippledSensor : Sensor {

		private const double CRIPPLED_COEFFICIENT = 0.08d;

		public override string Name {
			get { return "CrippledSensor"; }
		}

		public override object MeasuredData {
			get {
				return RandomUtils.NextDouble() < CRIPPLED_COEFFICIENT;
			}
		}
	}
}
