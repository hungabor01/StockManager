namespace ConsoleUI.EventArgs
{
    public class PriceRetrievedEventArgs : System.EventArgs
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }

        public PriceRetrievedEventArgs(string symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
        }
    }
}
