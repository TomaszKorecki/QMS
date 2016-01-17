using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using QmsLib.Interfaces;
using QMS.Models;

namespace QMS.Services {
	public class SensorsRepository {

		private static SensorsRepository instance;

		private SensorsRepository() {
			InvalidSensor = new ServerSensor<bool>();
			PregnantSensor = new ServerSensor<bool>();
			TemperatureSensor = new ServerSensor<float>();
			WeightSensor = new ServerSensor<float>();
		}

		public static SensorsRepository Instance {
			get {
				if (instance == null)
					instance = new SensorsRepository();
				return instance;
			}
		}

		public ServerSensor<bool> InvalidSensor { get; set; }
		public ServerSensor<bool> PregnantSensor { get; set; }
		public ServerSensor<float> TemperatureSensor { get; set; }
		public ServerSensor<float> WeightSensor { get; set; }

		public List<string> ConnectionIds {
			get {
				return new List<string>() {
					InvalidSensor.ConnectionId,
					PregnantSensor.ConnectionId,
					TemperatureSensor.ConnectionId,
					WeightSensor.ConnectionId
				};
			}
		}

		public bool ResetSensor(string connectionId) {
			if (InvalidSensor.ConnectionId == connectionId) {
				InvalidSensor.IsConnected = false;
				return true;
			}
			if (PregnantSensor.ConnectionId == connectionId) {
				PregnantSensor.IsConnected = false;
				return true;
			}
			if (TemperatureSensor.ConnectionId == connectionId) {
				TemperatureSensor.IsConnected = false;
				return true;
			}
			if (WeightSensor.ConnectionId == connectionId) {
				WeightSensor.IsConnected = false;
				return true;
			}

			return false;
		}
	}
}