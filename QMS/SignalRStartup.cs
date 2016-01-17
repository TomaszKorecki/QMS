using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using QMS;

[assembly: OwinStartup(typeof(SignalRStartup))]
namespace QMS {
	public class SignalRStartup {
		public void Configuration(IAppBuilder app) {
			// Any connection or hub wire up and configuration should go here
			app.MapSignalR("/signalR", new HubConfiguration());
		}
	}
}