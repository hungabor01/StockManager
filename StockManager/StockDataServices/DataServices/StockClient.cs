using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockDataServices.DataProviders;
using StockDataServices.Models;
using StockDataServices.Parameters;
using System;
using System.Collections.Generic;

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
    }
}
