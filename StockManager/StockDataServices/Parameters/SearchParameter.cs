namespace StockDataServices.Parameters
{
    internal class SearchParameter : IParameter
    {
        public string UrlPattern { get; } = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={1}&apikey={0}3&datatype=csv";
        public string Symbol { get; set; }

        public SearchParameter(string symbol)
        {
            Symbol = symbol;
        }
    }
}
