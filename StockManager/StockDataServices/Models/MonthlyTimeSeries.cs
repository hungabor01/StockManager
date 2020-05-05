namespace StockDataServices.Models
{
    internal class MonthlyTimeSeries:IModel
    {
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
    }
}
