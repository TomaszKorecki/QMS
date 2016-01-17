using System;

namespace QMS.Models {
	public class MeasuredData<T> {
		public T Data { get; set; }
		public DateTime MeasureTime { get; set; }
	}
}