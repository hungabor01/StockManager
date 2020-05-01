using StockDataServices.Parameters;
using StockDataServices.Models;
using System.Collections.Generic;

namespace StockDataServices.DataProviders
{
    internal interface IDataProvider
    {
        public T GetData<T>(IParameter paraemter) where T : IModel;
        public List<T> GetDataList<T>(IParameter parameter) where T : IModel;
    }
}
