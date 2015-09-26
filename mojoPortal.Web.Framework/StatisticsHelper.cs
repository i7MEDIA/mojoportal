

using System;
using System.Collections.Generic;

namespace mojoPortal.Web.Framework
{
    public static class StatisticsHelper
    {
        public static double Mean(IEnumerable<double> values)
        {
            double sum = 0;
            int count = 0;

            foreach (double d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        public static double StandardDeviation(IEnumerable<double> values, out double mean)
        {
            mean = Mean(values);
            double sumOfDiffSquares = 0;
            int count = 0;

            foreach (double d in values)
            {
                double diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return Math.Sqrt(sumOfDiffSquares / count);
        }

        public static double StandardDeviation(IEnumerable<double> values)
        {
            double mean;
            return StandardDeviation(values, out mean);
        }	

    }
}
