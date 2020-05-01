namespace StockDataServices.Models
{
    internal class BestMatch : IModel
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Type{ get; set; }
        public string Region{ get; set; }
        public string MarketOpen { get; set; }
        public string MarketClose { get; set; }
        public string Timezone { get; set; }
        public string Currency { get; set; }
        public string MatchScore { get; set; }        
    }
}
