using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QmsLib;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace TemperatureSensor {
	class TemperatureSensorProgram {
		static void Main(string[] args) {
			ISensor sensor = new TemperatureSensor();
			Console.WriteLine("Temperature sensor init");
			sensor.ConnectToServer();
			Console.WriteLine("Temperature sensor connected");
			Console.ReadLine();
			sensor.DisconnectFromServer();
		}
	}

	class TemperatureSensor : Sensor {

		private const double TEMPERATURE_MEAN = 36.6d;
		private const double TEMPERATURE_STDDEV = 0.3d;

		public override string Name {
			get { return "TemperatureSensor"; }
		}

		public override object MeasuredData {
			get { return RandomUtils.NextGaussian(TEMPERATURE_MEAN, TEMPERATURE_STDDEV); }
		}
	}
}
