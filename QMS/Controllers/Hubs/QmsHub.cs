using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNet.SignalR;
using Ninject;
using QmsLib.Models;
using QMS.Models;
using QMS.Services;

namespace QMS.Controllers.Hubs {
	public class QmsHub : Hub {

		SensorsRepository sensorsRepository { get; set; }
		QueuesRepository queuesRepository { get; set; }
		StatisticsService statisticsService { get; set; }

		public QmsHub() {
			sensorsRepository = SensorsRepository.Instance;
			queuesRepository = QueuesRepository.Instance;
			statisticsService = StatisticsService.Instance;
		}

		public void TestMessage(string name, string message) {
			Clients.All.testMessage(name, message);
		}

		public void RegisterQueue(string queueName, int queueId) {
			Clients.Caller.OnRegisterConfim();

			ServerQueue queue = new ServerQueue() {
				Name = queueName,
				Id = queueId,
				ConnectionId = Context.ConnectionId,
				IsOpened = true
			};

			lock (queuesRepository) {
				queuesRepository.Queues.Add(queueId, queue);
			}
		}

		public void RegisterSensor(string sensorName) {
			if (sensorName.StartsWith("Weight")) {
				sensorsRepository.WeightSensor.ConnectionId = Context.ConnectionId;
				sensorsRepository.WeightSensor.IsConnected = true;
			} else if (sensorName.StartsWith("Crippled")) {
				sensorsRepository.CrippledSensor.ConnectionId = Context.ConnectionId;
				sensorsRepository.CrippledSensor.IsConnected = true;
			} else if (sensorName.StartsWith("Temperature")) {
				sensorsRepository.TemperatureSensor.ConnectionId = Context.ConnectionId;
				sensorsRepository.TemperatureSensor.IsConnected = true;
			} else if (sensorName.StartsWith("Pregnant")) {
				sensorsRepository.PregnantSensor.ConnectionId = Context.ConnectionId;
				sensorsRepository.PregnantSensor.IsConnected = true;
			}
		}

		public void RegisterBrowser() {
			BrowserStateRepository.ConnectionId = Context.ConnectionId;
		}

		public void DataFromSensor(SensorData sensorData) {
			var measuredData = sensorData.Data;
			var sensorName = sensorData.SensorName;
			var browserConnectionId = BrowserStateRepository.ConnectionId;


			if (statisticsService.UnorderedPerson == null) {
				statisticsService.AssignNewUnorderedPerson();
			}

			//data from sensors logic
			if (sensorName.StartsWith("Weight")) {
				var data = new MeasuredData<float>() { Data = Convert.ToSingle(measuredData), MeasureTime = DateTime.Now };
				sensorsRepository.WeightSensor.AddMeasuredData(data);

				statisticsService.UnorderedPersonWeight = data.Data;

				if (!String.IsNullOrEmpty(browserConnectionId))
					Clients.Client(browserConnectionId).onNewSensorDataReceived("weight", data);
				//weight is a trigger
				RequestDataFromSensors();

			} else if (sensorName.StartsWith("Crippled")) {
				var data = new MeasuredData<bool>() { Data = Convert.ToBoolean(measuredData), MeasureTime = DateTime.Now };
				sensorsRepository.CrippledSensor.AddMeasuredData(data);

				statisticsService.UnorderedPersonCrippled = data.Data;

				if (!String.IsNullOrEmpty(browserConnectionId))
					Clients.Client(browserConnectionId).onNewSensorDataReceived("crippled", data);
			} else if (sensorName.StartsWith("Temperature")) {
				var data = new MeasuredData<float>() { Data = Convert.ToSingle(measuredData), MeasureTime = DateTime.Now };
				sensorsRepository.TemperatureSensor.AddMeasuredData(data);

				statisticsService.UnorderedPersonTemperature = data.Data;

				if (!String.IsNullOrEmpty(browserConnectionId))
					Clients.Client(browserConnectionId).onNewSensorDataReceived("temperature", data);
			} else if (sensorName.StartsWith("Pregnant")) {
				var data = new MeasuredData<bool>() { Data = Convert.ToBoolean(measuredData), MeasureTime = DateTime.Now };
				sensorsRepository.PregnantSensor.AddMeasuredData(data);

				statisticsService.UnorderedPersonPregnant = data.Data;

				if (!String.IsNullOrEmpty(browserConnectionId))
					Clients.Client(browserConnectionId).onNewSensorDataReceived("pregnant", data);
			}

			if (statisticsService.CheckIfUnorderedPersonCanBeDirected()) {
				var person = statisticsService.GetUnorderedPersonComplete();
				var queue = queuesRepository.ChooseQueueForPerson(person);

				Clients.Client(queue.ConnectionId).AddPersonToQueue(person.Guid);

				if (!String.IsNullOrEmpty(browserConnectionId))
					Clients.Client(browserConnectionId).updateQueueLogs(queue);

				StatisticsService.Instance.HandledPeople.Add(person.Guid, new HandledPerson() {
					Person = person,
					QueueId = queue.Id,
					WaitingStartedAt = DateTime.Now
				});

				statisticsService.UnorderedPerson = null;
				statisticsService.UnorderedPersonCrippled = null;
				statisticsService.UnorderedPersonPregnant = null;
				statisticsService.UnorderedPersonTemperature = null;
				statisticsService.UnorderedPersonWeight = null;
			}
		}

		public void RequestDataFromSensors() {
			if (!String.IsNullOrEmpty(BrowserStateRepository.ConnectionId)) {
				var sensorsIds = sensorsRepository.ConnectionIds;
				sensorsIds.Remove(sensorsRepository.WeightSensor.ConnectionId);
				Clients.Clients(sensorsIds).OnReceiveDataRequestFromServer();

				Clients.Client(BrowserStateRepository.ConnectionId).onRequestToSensorSent("temperature", DateTime.Now);
				Clients.Client(BrowserStateRepository.ConnectionId).onRequestToSensorSent("pregnant", DateTime.Now);
				Clients.Client(BrowserStateRepository.ConnectionId).onRequestToSensorSent("crippled", DateTime.Now);
			}
		}


		public void PersonHandledInQueue(int queueId, WaitingPersonMeta personMeta) {
			queuesRepository.RemovePersonFromQueue(personMeta, queueId);

			var handledPerson = StatisticsService.Instance.HandledPeople[personMeta.PersonGuid];
			handledPerson.WaitingTime = personMeta.StopWaitingTime - personMeta.StartWaitingTime;

			if (!String.IsNullOrEmpty(BrowserStateRepository.ConnectionId)) {
				var queue = queuesRepository.Queues[queueId];
				Clients.Client(BrowserStateRepository.ConnectionId).updateQueueLogs(queue);
			}
		}

		public void ChangeQueueState(int queueId, bool open) {
			if (open) {
				queuesRepository.StartQueue(queueId);
			} else {
				queuesRepository.StopQueue(queueId);
			}
		}

		public override Task OnDisconnected(bool stopCalled) {
			string id = Context.ConnectionId;

			bool sensorResetted = sensorsRepository.ResetSensor(id);
			if (!sensorResetted) {
				queuesRepository.ResetQueue(id);
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}