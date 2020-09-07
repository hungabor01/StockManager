using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockDataServices.DataProviders;
using StockDataServices.Models;
using StockDataServices.Parameters;
using System;
using System.Collections.Generic;

namespace StockDataServices.DataServices
{
    public class StockClientOptions
    {
        public string ApiKey { get; set; }
        public int DeviationCalculationRange { get; set; }
    }

    public class StockClient : IStockClient
    {
        private readonly int _deviationCalculationRange;

        private readonly IDataProvider _dataProvider;
        private readonly ILogger<StockClient> _logger;

        public StockClient(IOptions<StockClientOptions> options, ILogger<StockClient> logger)
        {
            _logger = logger;

            if (options == null || options.Value == null || string.IsNullOrWhiteSpace(options.Value.ApiKey))
            {
                throw new ArgumentNullException(nameof(options.Value.ApiKey));
            }
            
            var apikey = options.Value.ApiKey;
            _dataProvider = new AlphaVantageDataProvider(apikey);

            _deviationCalculationRange = options.Value.DeviationCalculationRange;
        }

        public decimal? GetPrice(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            try
            {
                var parameter = new GlobalQuoteParameter(symbol);
                var result = _dataProvider.GetData<GlobalQuote>(parameter);
                return result?.Price;
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

            try
            {
                var parameter = new SearchParameter(symbol);
                var result = _dataProvider.GetDataList<BestMatch>(parameter);

                if (result != null)
                {
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
            } 
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to search for {symbol}. {e.Message}");
            }

            return matchList;
        }        

        public List<decimal> GetPricesAndDeviations(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var prices = new List<decimal>();

            try
            {
                var parameter = new DailyTimeSeriesParameter(symbol);
                var result = _dataProvider.GetDataList<TimeSeries>(parameter);

                if (result != null)
                {
                    foreach (var timeSeries in result)
                    {
                        if (timeSeries.TimeStamp.AddDays(_deviationCalculationRange) > DateTime.Today)
                        {
                            prices.Add(timeSeries.Close);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get prices and deviations for {symbol}. {e.Message}");
            }

            return prices;            
        }
    }
}
