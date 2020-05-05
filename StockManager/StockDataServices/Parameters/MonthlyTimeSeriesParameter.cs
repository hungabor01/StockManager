using System;
using System.Collections.Generic;
using System.Text;

namespace StockDataServices.Parameters
{
    internal class MonthlyTimeSeriesParameter : IParameter
    {
        public string UrlPattern { get; } = "https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={1}&apikey={0}&datatype=csv";

        public string Symbol { get; set; }
        public MonthlyTimeSeriesParameter(String symbol)
        {
            Symbol = symbol;
        }
    }

}
