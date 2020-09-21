using System.Collections.Generic;

namespace StockDataServices.DataServices
{
    public interface IStockClient
    {
        public decimal? GetStockPrice(string symbol);
        public string[] GetStockData(string symbol);
        public List<string[]> Search(string symbol);
        public List<decimal> GetPricesAndDeviations(string symbol);
    }
}
