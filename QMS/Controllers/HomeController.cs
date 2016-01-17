using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web.Mvc;
using Ninject;
using QMS.Models;
using QMS.Models.ViewModels;
using QMS.Services;

namespace QMS.Controllers {
	public class HomeController : Controller {

		public SensorsRepository sensorsRepository { get; set; }
		public QueuesRepository queuesRepository { get; set; }

		public HomeController() {
			sensorsRepository = SensorsRepository.Instance;
			queuesRepository = QueuesRepository.Instance;
		}

		public ActionResult Index() {
			HomeViewModel model = new HomeViewModel() {
				QueuesRepository = queuesRepository,
				SensorsRepository = sensorsRepository
			};

			return View(model);
		}

		public ActionResult LastSensorMessages(DateTime fromDateTime) {
			return Json(new {
				temperatureLogs = sensorsRepository.TemperatureSensor.MeasuredData.Where(x => x.MeasureTime > fromDateTime),
				weightLogs = sensorsRepository.WeightSensor.MeasuredData.Where(x => x.MeasureTime > fromDateTime),
				invalidLogs = sensorsRepository.InvalidSensor.MeasuredData.Where(x => x.MeasureTime > fromDateTime),
				pregnantLogs = sensorsRepository.PregnantSensor.MeasuredData.Where(x => x.MeasureTime > fromDateTime)
			});
		}


		public ActionResult About() {
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}