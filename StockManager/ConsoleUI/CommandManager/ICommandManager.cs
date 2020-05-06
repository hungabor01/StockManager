using ConsoleUI.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleUI.CommandManager
{
    public interface ICommandManager
    {
        public event EventHandler<PriceRetrievedEventArgs> PriceRerieved;

        public Task GetPrices();
        public List<string[]> Search(string symbol);
        public List<string[]> GetMonthlyPrice(string symbol);
        public List<string> GetDailyPriceAndDeviation(string symbol);
    }
}
