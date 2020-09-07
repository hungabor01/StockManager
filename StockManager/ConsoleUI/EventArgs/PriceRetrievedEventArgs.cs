using ConsoleUI.Models;

namespace ConsoleUI.EventArgs
{
    public class PriceRetrievedEventArgs : System.EventArgs
    {
        public Stock Stock { get; set; }

        public PriceRetrievedEventArgs(Stock stock)
        {
            Stock = stock;
        }
    }
}
