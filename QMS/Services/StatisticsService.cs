using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QmsLib.Models;
using QMS.Models;

namespace QMS.Services {
	public class StatisticsService {
		private static StatisticsService instance;
		public static StatisticsService Instance {
			get { return instance ?? (instance = new StatisticsService()); }
		}

		public Person UnorderedPerson { get; set; }
		public bool? UnorderedPersonCrippled { get; set; }
		public bool? UnorderedPersonPregnant { get; set; }
		public float? UnorderedPersonWeight { get; set; }
		public float? UnorderedPersonTemperature { get; set; }

		public Dictionary<Guid, HandledPerson> HandledPeople = new Dictionary<Guid, HandledPerson>(); 

		public bool CheckIfUnorderedPersonCanBeDirected() {
			if (UnorderedPersonCrippled.HasValue && UnorderedPersonPregnant.HasValue && UnorderedPersonWeight.HasValue &&
				UnorderedPersonTemperature.HasValue && UnorderedPerson != null) {
				return true;
			}

			return false;
		}

		public void AssignNewUnorderedPerson() {
			UnorderedPerson = new Person() {
				Guid = Guid.NewGuid()
			};
		}

		public Person GetUnorderedPersonComplete() {
			UnorderedPerson.IsCrippled = UnorderedPersonCrippled.Value;
			UnorderedPerson.IsPregnant = UnorderedPersonPregnant.Value;
			UnorderedPerson.Weight = UnorderedPersonWeight.Value;
			UnorderedPerson.Temperature = UnorderedPersonTemperature.Value;

			return UnorderedPerson;
		}
	}
}