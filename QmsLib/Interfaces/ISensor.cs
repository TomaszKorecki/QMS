using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QmsLib.Interfaces {
	public interface ISensor {
		string Name { get; }
		object MeasuredData { get; }

		void ConnectToServer();
		void DisconnectFromServer();
		void OnConnectedToServer();
		void SendDataToServer();
		void OnReceiveDataRequestFromServer();
	}
}
