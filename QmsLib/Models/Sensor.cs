using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using QmsLib.Interfaces;

namespace QmsLib.Models {
	public abstract class Sensor : ISensor {
		private static int MinWaitingTime = 500;
		private static int MaxWaitingTime = 2000;

		protected HubConnection hubConnection;
		protected IHubProxy hubProxy;

		public virtual string Name {
			get { return "Abstract sensor"; }
		}

		public virtual object MeasuredData {
			get { return "Measured data"; }
		}

		public virtual void ConnectToServer() {
			hubConnection = new HubConnection("http://localhost:52807/signalR", false);
			hubProxy = hubConnection.CreateHubProxy("QmsHub");

			hubProxy.On("OnReceiveDataRequestFromServer", OnReceiveDataRequestFromServer);

			hubConnection.Start().Wait();
			OnConnectedToServer();
		}

		public virtual void DisconnectFromServer() {
			hubConnection.Stop();
		}

		public virtual void OnConnectedToServer() {
			RegisterOnServer();
		}

		protected virtual void RegisterOnServer() {
			hubProxy.Invoke("RegisterSensor", Name);
		}

		public virtual void SendDataToServer() {
			if (hubConnection.State == ConnectionState.Connected) {
				hubProxy.Invoke("DataFromSensor", new SensorData {
					Data = MeasuredData,
					SensorName = Name
				});
			}
		}

		public virtual void OnReceiveDataRequestFromServer() {
			Random random = new Random(Name.ToCharArray().Select(x => (int)x).Sum());
			Thread.Sleep(random.Next(MinWaitingTime, MaxWaitingTime));
			SendDataToServer();
		}
	}
}
