using System.Collections.Generic;

namespace ConsoleUI.CommandManager
{
    public interface ICommandManager
    {
        public void GetPrices();
        public List<string[]> Search(string symbol);
    }
}
