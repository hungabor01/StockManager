using ConsoleUI.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleUI.CommandManager
{
    public interface ICommandManager
    {
        public event EventHandler<PriceRetrievedEventArgs> PriceRerieved;

        public Task GetPricesAndDeviation();
        public List<string[]> Search(string symbol);
    }
}
