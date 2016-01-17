using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QmsLib.Interfaces {
	public interface IInstitutionQueue {
		string Name { get; set; }
		bool IsRegistered { get; set; }

		void ConnectToServer();
		void OnConnectedToServer();
	}
}
