using QMS.Services;

namespace QMS.Models.ViewModels {
	public class HomeViewModel {
		public SensorsRepository SensorsRepository { get; set; }
		public QueuesRepository QueuesRepository { get; set; }
	}
}