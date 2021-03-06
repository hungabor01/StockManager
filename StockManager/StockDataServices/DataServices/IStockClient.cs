﻿using System.Collections.Generic;

namespace StockDataServices.DataServices
{
    public interface IStockClient
    {
        public decimal? GetPrice(string symbol);
        public List<string[]> Search(string symbol);
        public List<decimal> GetPricesAndDeviations(string symbol);
    }
}
