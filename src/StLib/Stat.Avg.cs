using System.Collections.Generic;

namespace StLib
{
    public static partial class Stat
    {
        public static double Avg(double prev, double next, double n)
        {
            return ((n - 1) / n) * prev + next / n;
        }

        public static double Avg(this IEnumerable<double> values)
        {
            double avg = 0;
            double n = 1;
            foreach (var v in values)
            {
                avg = Avg(avg, v, n);
                n++;
            }
            return avg;
        }
    }
}
