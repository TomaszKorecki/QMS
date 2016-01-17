using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using QmsLib.Interfaces;

namespace QmsLib.Models {
	public class InstitutionQueue : IInstitutionQueue {
		protected HubConnection hubConnection;
		protected IHubProxy hubProxy;

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsRegistered { get; set; }

		private Queue<Guid> peopleInQueue = new Queue<Guid>();

		private static int MinPersonHandlingTime = 15000;
		private static int MaxPersonHandlingTime = 24000;

		public virtual void ConnectToServer() {
			hubConnection = new HubConnection("http://localhost:52807/signalR", false);
			hubProxy = hubConnection.CreateHubProxy("QmsHub");

			hubProxy.On("OnRegisterConfirm", OnRegisteConfirm);
			hubProxy.On<Guid>("AddPersonToQueue", AddPersonToQueue);

			hubConnection.Start().Wait();
			OnConnectedToServer();
		}

		public virtual void OnConnectedToServer() {
			RegisterOnServer();

			Task.Run(() => {
				while (true) {
					if (peopleInQueue.Count > 0) {
						Random random = new Random(DateTime.Now.Millisecond);
						Thread.Sleep(random.Next(MinPersonHandlingTime, MaxPersonHandlingTime));

						lock (peopleInQueue) {
							var handled = peopleInQueue.Dequeue();
							hubProxy.Invoke("PersonHandledInQueue", Id, handled);
						}
					}

					Thread.Sleep(100);
				}
			});
		}

		protected virtual void RegisterOnServer() {
			hubProxy.Invoke("RegisterQueue", Name, Id);
		}

		public void OnRegisteConfirm() {
			IsRegistered = true;
		}

		public void AddPersonToQueue(Guid guid) {
			lock (peopleInQueue) {
				peopleInQueue.Enqueue(guid);
			}
		}

		public void RemovePersonFromQueue() {
			var guid = peopleInQueue.Dequeue();
			hubProxy.Invoke("RemovePersonFromQueue", Id, guid);
		}
	}
}
