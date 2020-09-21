using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class ExtensionMethods
    {
        public static void ThrowExceptionIfNull(this object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ThrowExceptionIfNullOrWhiteSpace(this string str, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException(parameterName);
            }
        }

        public static void ThrowExceptionIfOptionNotValid<T>(this IOptions<T> options, string parameterName)
            where T : class, new()
        {
            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static double GetDeviation(this IEnumerable<decimal> prices)
        {
            var average = prices.Average();
            var sum = prices.Sum(d => (d - average) * (d - average));
            var deviation = Math.Sqrt((double)sum / prices.Count());
            return Math.Round(deviation, 4);
        }
    }
}
