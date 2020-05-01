namespace StockDataServices.Parameters
{
    internal interface IParameter
    {
        public string UrlPattern { get; }
        public string Symbol { get; set; }
    }
}
