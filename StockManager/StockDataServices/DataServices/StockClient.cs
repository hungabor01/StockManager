using Common;
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

            options.ThrowExceptionIfOptionNotValid(nameof(StockClientOptions));
            options.Value.ApiKey.ThrowExceptionIfNullOrWhiteSpace(nameof(options.Value.ApiKey));
            
            var apikey = options.Value.ApiKey;
            _dataProvider = new AlphaVantageDataProvider(apikey);

            _deviationCalculationRange = options.Value.DeviationCalculationRange > 0 ? options.Value.DeviationCalculationRange : 30;
        }

        public decimal? GetStockPrice(string symbol)
        {
            symbol.ThrowExceptionIfNullOrWhiteSpace(nameof(symbol));

            try
            {
                var parameter = new GlobalQuoteParameter(symbol);
                var result = _dataProvider.GetData<GlobalQuote>(parameter);
                return result?.Price;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get the price for {symbol}. {e.Message}");
                return null;
            }
        }

        public string[] GetStockData(string symbol)
        {
            symbol.ThrowExceptionIfNullOrWhiteSpace(nameof(symbol));            

            try
            {
                var parameter = new GlobalQuoteParameter(symbol);
                var result = _dataProvider.GetData<GlobalQuote>(parameter);                

                if (result == null)
                {
                    return null;
                }

                return new string[] {
                    result.Symbol,
                    result.Open.ToString(),
                    result.High.ToString(),
                    result.Low.ToString(),
                    result.Price.ToString(),
                    result.Volume.ToString(),
                    result.LatestTradingDay,
                    result.PreviousClose.ToString(),
                    result.Change.ToString(),
                    result.ChangePercent
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get the data for {symbol}. {e.Message}");
                return null;
            }
        }

        public List<string[]> Search(string symbol)
        {
            symbol.ThrowExceptionIfNullOrWhiteSpace(nameof(symbol));            

            try
            {
                var parameter = new SearchParameter(symbol);
                var result = _dataProvider.GetDataList<BestMatch>(parameter);

                if (result == null)
                {
                    return null;
                }

                var matchList = new List<string[]>();
                foreach (var match in result)
                {
                    matchList.Add(new string[] {
                        match.Symbol,
                        match.Name,
                        match.Region,
                        match.Currency
                    });
                }
                return matchList;
            } 
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to search for {symbol}. {e.Message}");
                return null;
            }            
        }        

        public List<decimal> GetPricesAndDeviations(string symbol)
        {
            symbol.ThrowExceptionIfNullOrWhiteSpace(nameof(symbol));

            try
            {
                var parameter = new DailyTimeSeriesParameter(symbol);
                var result = _dataProvider.GetDataList<TimeSeries>(parameter);

                if (result == null)
                {
                    return null;
                }

                var prices = new List<decimal>();
                foreach (var timeSeries in result)
                {
                    if (timeSeries.TimeStamp.AddDays(_deviationCalculationRange) > DateTime.Today)
                    {
                        prices.Add(timeSeries.Close);
                    }
                }
                return prices;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get prices and deviations for {symbol}. {e.Message}");
                return null;
            }            
        }        
    }  
}
