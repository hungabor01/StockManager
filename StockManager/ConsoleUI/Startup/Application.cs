using ConsoleUI.CommandManager;
using System;

namespace StockDataProvider.Startup
{
    public class Application : IApplication
    {
        private readonly ICommandManager _commandManager;

        public Application(ICommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        public void Run()
        {
            Console.WriteLine("To search for a symbol of a stock, type: search [symbol]");
            Console.WriteLine("To get the prices to the symbols in the file, type: getprices");
            Console.WriteLine("To quit, type: quit");

            string command;
            while((command = Console.ReadLine().ToLower()) != "quit")
            {
                if (command == "getprices")
                {
                    _commandManager.GetPrices();
                    Console.WriteLine("The prices were successfully downloaded.");
                }
                else if (command.Contains("search"))
                {
                    var symbol = command.Substring(7);
                    var matches = _commandManager.Search(symbol);

                    if (matches != null && matches.Count > 0)
                    {
                        foreach (var match in matches)
                        {
                            Console.WriteLine(string.Join('\t', match));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No result for {symbol}.");
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }            

            Console.WriteLine("Press any key to close the application.");
            Console.ReadKey();
        }
    }
}
