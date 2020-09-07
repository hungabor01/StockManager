using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static double GetDeviation(this IEnumerable<decimal> prices)
        {
            var average = prices.Average();
            var sum = prices.Sum(d => (d - average) * (d - average));
            var deviation = Math.Sqrt((double)sum / prices.Count());
            return Math.Round(deviation, 4);
        }
    }
}
