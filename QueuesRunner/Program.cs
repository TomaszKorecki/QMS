using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QmsLib.Interfaces;
using QmsLib.Models;

namespace QueuesRunner {
	class Program {
		static void Main(string[] args) {

			IInstitutionQueue queue1 = new InstitutionQueue() {
				Id = 1,
				Name = "First queue"
			};
			IInstitutionQueue queue2 = new InstitutionQueue() {
				Id = 2,
				Name = "Second queue"
			};
			IInstitutionQueue queue3 = new InstitutionQueue() {
				Id = 3,
				Name = "Third queue"
			};

			queue1.ConnectToServer();
			queue2.ConnectToServer();
			queue3.ConnectToServer();

			Console.WriteLine("Queue 1 connected");
			Console.WriteLine("Queue 2 connected");
			Console.WriteLine("Queue 3 connected");

			Console.ReadLine();
		}
	}
}
