using System.Collections.Generic;

namespace QMS.Models {
	public class ServerSensor<T> {
		private List<MeasuredData<T>> measuredData;

		public ServerSensor() {
			measuredData = new List<MeasuredData<T>>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsConnected { get; set; }
		public string ConnectionId { get; set; }

		public List<MeasuredData<T>> MeasuredData {
			get { return measuredData; }
		}

		public void AddMeasuredData(MeasuredData<T> data) {
			measuredData.Add(data);
		}
	}
}