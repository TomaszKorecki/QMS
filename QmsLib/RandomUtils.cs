using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QmsLib {
	public class RandomUtils {
		private static Random random = new Random(); //(GenerateSeed());

		public static int NextInt(int max) {
			return random.Next(max);
		}

		public static int NextInt(int min, int max) {
			return random.Next(min, max);
		}

		public static double NextDouble() {
			return random.NextDouble();
		}

		public static double NextGaussian(double mean, double stdDev) {
			double u1 = random.NextDouble(); //these are uniform(0,1) random doubles
			double u2 = random.NextDouble();
			double randStdNormal = Math.Sqrt(-2.0*Math.Log(u1))*Math.Sin(2.0*Math.PI*u2); //random normal(0,1)
			double randNormal = mean + stdDev*randStdNormal; //random normal(mean,stdDev^2)
			return randNormal;
		}

		public static double NextGaussian(double mean, double stdDev, double min, double max) {
			double u1 = random.NextDouble(); //these are uniform(0,1) random doubles
			double u2 = random.NextDouble();
			double randStdNormal = Math.Sqrt(-2.0*Math.Log(u1))*Math.Sin(2.0*Math.PI*u2); //random normal(0,1)
			double randNormal = mean + stdDev*randStdNormal; //random normal(mean,stdDev^2)

			if (randNormal < min) {
				return min + (NextDouble()*(max - min)/10d);
			} else if (randNormal > max) {
				return max - (NextDouble()*(max - min)/10d);
			} else {
				return randNormal;
			}
		}

		public static int GenerateSeed() {
			return
				(int) DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}
