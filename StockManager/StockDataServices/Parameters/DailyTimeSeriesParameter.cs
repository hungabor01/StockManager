using System;
using System.Collections.Generic;
using System.Text;

namespace StockDataServices.Parameters
{
    internal class DailyTimeSeriesParameter : IParameter
    {
        public string UrlPattern { get; } = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={1}&apikey={0}&datatype=csv";

        public string Symbol { get; set; }

        public DailyTimeSeriesParameter(string symbol)
        {
            Symbol = symbol;
        }

    }
}
