using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QmsLib.Models;

namespace QMS.Services {
	public class StatisticsService {
		private static StatisticsService instance;
		public static StatisticsService Instance {
			get { return instance ?? (instance = new StatisticsService()); }
		}


		public Person UnorderedPerson { get; set; }
		public bool? UnorderedPersonInvalid { get; set; }
		public bool? UnorderedPersonPregnant { get; set; }
		public float? UnorderedPersonWeight { get; set; }
		public float? UnorderedPersonTemperature { get; set; }


		public bool CheckIfUnorderedPersonCanBeDirected() {
			if (UnorderedPersonInvalid.HasValue && UnorderedPersonPregnant.HasValue && UnorderedPersonWeight.HasValue &&
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
			UnorderedPerson.IsInvalid = UnorderedPersonInvalid.Value;
			UnorderedPerson.IsPregnant = UnorderedPersonPregnant.Value;
			UnorderedPerson.Weight = UnorderedPersonWeight.Value;
			UnorderedPerson.Temperature = UnorderedPersonTemperature.Value;

			return UnorderedPerson;
		}


	}
}