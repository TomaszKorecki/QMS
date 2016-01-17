using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QmsLib;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace PregnantSensor {
	class PregnantSensorProgram {
		static void Main(string[] args) {
			ISensor sensor = new PregnantSensor();
			Console.WriteLine("Pregnant sensor init");
			sensor.ConnectToServer();
			Console.WriteLine("Pregnant sensor connected");
			Console.ReadLine();
			sensor.DisconnectFromServer();
		}
	}

	class PregnantSensor : Sensor {

		private const double PREGNANT_COEFFICIENT = 0.009770294d;

		public override string Name {
			get { return "Pregnant sensor"; }
		}

		public override object MeasuredData {
			get {
				return RandomUtils.NextDouble() < PREGNANT_COEFFICIENT;
			}
		}
	}
}
