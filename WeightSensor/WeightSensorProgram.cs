using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QmsLib;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace WeightSensor {
	class WeightSensorProgram {
		static void Main(string[] args) {
			ISensor sensor = new WeightSensor();
			Console.WriteLine("Weight sensor init");
			sensor.ConnectToServer();
			Console.WriteLine("Weight sensor connected");


			int minDelayBetweenPeople = 5000;
			int maxDelayBetweenPeople = 8000;

			for (int i = 0; i < 3000; i++) {
				sensor.SendDataToServer();
				Console.WriteLine("Sending new weight data");
				Random random = new Random(DateTime.Now.Millisecond);
				int wait = random.Next(minDelayBetweenPeople, maxDelayBetweenPeople);
				Thread.Sleep(wait);
			}

			Console.ReadLine();
			sensor.DisconnectFromServer();
		}
	}

	class WeightSensor : Sensor {

		private const double WEIGHT_MEAN = 70d;
		private const double WEIGHT_STDDEV = 10d;
		private const double WEIGHT_MIN = 25d;
		private const double WEIGHT_MAX = 200d;

		public override string Name {
			get { return "WeightSensor"; }
		}

		public override object MeasuredData {
			get { return RandomUtils.NextGaussian(WEIGHT_MEAN, WEIGHT_STDDEV, WEIGHT_MIN, WEIGHT_MAX); }
		}
	}
}
