using System;
using System.Linq;
using System.Threading.Tasks;
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

			queuesRepository.Queues.Add(new ServerQueue() {
				Name = queueName,
				Id = queueId,
				ConnectionId = Context.ConnectionId
			});
		}

		public void RegisterSensor(string sensorName) {
			if (sensorName.StartsWith("Weight")) {
				sensorsRepository.WeightSensor.ConnectionId = Context.ConnectionId;
				sensorsRepository.WeightSensor.IsConnected = true;
			} else if (sensorName.StartsWith("Invalid")) {
				sensorsRepository.InvalidSensor.ConnectionId = Context.ConnectionId;
				sensorsRepository.InvalidSensor.IsConnected = true;
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

			} else if (sensorName.StartsWith("Invalid")) {
				var data = new MeasuredData<bool>() { Data = Convert.ToBoolean(measuredData), MeasureTime = DateTime.Now };
				sensorsRepository.InvalidSensor.AddMeasuredData(data);

				statisticsService.UnorderedPersonInvalid = data.Data;

				if (!String.IsNullOrEmpty(browserConnectionId))
					Clients.Client(browserConnectionId).onNewSensorDataReceived("invalid", data);
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

				statisticsService.UnorderedPerson = null;
				statisticsService.UnorderedPersonInvalid = null;
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
				Clients.Client(BrowserStateRepository.ConnectionId).onRequestToSensorSent("invalid", DateTime.Now);
			}
		}


		public void PersonHandledInQueue(int queueId, Guid guid) {
			queuesRepository.RemovePersonFromQueue(guid, queueId);
			if (!String.IsNullOrEmpty(BrowserStateRepository.ConnectionId)) {
				var queue = queuesRepository.Queues.FirstOrDefault(x => x.Id == queueId);
				Clients.Client(queue.ConnectionId).updateQueueLogs(queue);
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