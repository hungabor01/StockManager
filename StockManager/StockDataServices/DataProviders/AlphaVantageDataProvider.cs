using ServiceStack;
using StockDataServices.Parameters;
using StockDataServices.Models;
using System.Collections.Generic;

namespace StockDataServices.DataProviders
{
    internal class AlphaVantageDataProvider : IDataProvider
    {
        private readonly string _apiKey;

        public AlphaVantageDataProvider(string apikey)
        {
            _apiKey = apikey;
        }

        public T GetData<T>(IParameter parameter) where T : IModel
        {
            var url = string.Format(parameter.UrlPattern, _apiKey, parameter.Symbol);            
            return url.GetStringFromUrl().FromCsv<T>();            
        }

        public List<T> GetDataList<T>(IParameter parameter) where T : IModel
        {
            var url = string.Format(parameter.UrlPattern, _apiKey, parameter.Symbol);
            return url.GetStringFromUrl().FromCsv<List<T>>();            
        }
    }
}