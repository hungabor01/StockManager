using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockDataServices.DataProviders;
using StockDataServices.Models;
using StockDataServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockDataServices.DataServices
{
    public class StockClientSettings
    {
        public string ApiKey { get; set; }
    }

    public class StockClient : IStockClient
    {
        private readonly IDataProvider _dataProvider;
        private readonly ILogger<StockClient> _logger;

        public StockClient(IOptions<StockClientSettings> options, ILogger<StockClient> logger)
        {
            _logger = logger;

            if (options == null || options.Value == null || string.IsNullOrWhiteSpace(options.Value.ApiKey))
            {
                throw new ArgumentNullException(nameof(options.Value.ApiKey));
            }
            
            var apikey = options?.Value.ApiKey;
            _dataProvider = new AlphaVantageDataProvider(apikey);
        }

        public decimal? GetPrice(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var parameter = new GlobalQuoteParameter(symbol);

            try
            {
                var result = _dataProvider.GetData<GlobalQuote>(parameter);
                return result.Price;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to retrieve the price for {symbol}. {e.Message}");
                return null;
            }
        }

        public List<string[]> Search(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var matchList = new List<string[]>();

            var parameter = new SearchParameter(symbol);

            try
            {
                var result = _dataProvider.GetDataList<BestMatch>(parameter);

                foreach (var match in result)
                {
                    matchList.Add(new string[] {
                        match.Symbol,
                        match.Name,
                        match.Region,
                        match.Currency
                    });
                }
            } 
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to search for {symbol}. {e.Message}");
            }

            return matchList;
        }
        public List<string[]> GetMonthlyPrice(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var matchList = new List<string[]>();

            var parameter = new MonthlyTimeSeriesParameter(symbol);

            try
            {
                var result = _dataProvider.GetDataList<TimeSeries>(parameter);

                foreach (var match in result)
                {
                    matchList.Add(new string[] {
                        match.TimeStamp.ToString(),
                        match.High.ToString(),
                        match.Low.ToString(),
                        match.Open.ToString(),
                        match.Close.ToString(),
                        match.Volume.ToString()
                    }) ;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to search for {symbol}. {e.Message}");
            }

            return matchList;
        }
        public List<string> GetDailyPriceAndDeviation(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var matchList = new List<double>();

            var parameter = new DailyTimeSeriesParameter(symbol);

            var priceAndDeviation = new List<String>();

            try
            {
                List<TimeSeries> result = _dataProvider.GetDataList<TimeSeries>(parameter);

                priceAndDeviation.Add(result[0].Close.ToString());

                for (int i=0;i<result.Count;i++)
                {
                    if (result[i].TimeStamp.AddMonths(1) < DateTime.Today)
                    {
                        break;
                    }
                    matchList.Add((double)result[i].Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to search for {symbol}. {e.Message}");
            }

            double avg = matchList.Average();
            priceAndDeviation.Add(Math.Sqrt(matchList.Average(v => Math.Pow(v - avg, 2))).ToString());
            return priceAndDeviation;
        }
    }

}
