namespace StockDataServices.Parameters
{
    internal class GlobalQuoteParameter : IParameter
    {
        public string UrlPattern { get; } = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={1}&apikey={0}&datatype=csv";
        public string Symbol { get; set; }

        public GlobalQuoteParameter(string symbol)
        {
            Symbol = symbol;
        }
    }
}
