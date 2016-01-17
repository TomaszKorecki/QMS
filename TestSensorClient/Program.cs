using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Serialization;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace TestSensorClient {
	class Program {
		static void Main(string[] args) {

			ISensor sensor = new TestSensor();
			sensor.ConnectToServer();
		}
	}


}
