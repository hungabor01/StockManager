using ConsoleUI.CommandManager;
using ConsoleUI.EventArgs;
using System;
using System.Threading.Tasks;

namespace StockDataProvider.Startup
{
    public class Application : IApplication
    {
        private const char PricesIdentifier = 'p';
        private const char SearchIdentifier = 's';
        private const string QuithIdentifier = "q";

        private readonly ICommandManager _commandManager;

        public Application(ICommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        public async Task Run()
        {
            Console.WriteLine($"To get the prices and deviations to the symbols in the file, type: {PricesIdentifier}");
            Console.WriteLine($"To search for a symbol of a stock, type: {SearchIdentifier} [symbol]");
            Console.WriteLine("To quit from the application, type: q");

            string command;
            while ((command = Console.ReadLine().ToLower()) != QuithIdentifier)
            {
                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                switch (command[0])
                {
                    case 'p':
                        await GetPricesAndDeviation();
                        break;
                    case 's':
                        Search(command);
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private async Task GetPricesAndDeviation()
        {
            _commandManager.PriceRerieved += OnPriceRetrieved;
            await _commandManager.GetPricesAndDeviation();
            Console.WriteLine("Finished downloading the prices and deviatons.");
        }

        private void Search(string command)
        {
            if (command.Length <= 2)
            {
                Console.WriteLine("No symbol entered.");
                return;
            }

            var symbol = command.Substring($"{SearchIdentifier} ".Length);

            var matches = _commandManager.Search(symbol);

            if (matches == null || matches.Count == 0)
            {
                Console.WriteLine($"No result for {symbol}.");
                return;
            }
            
            foreach (var match in matches)
            {
                Console.WriteLine(string.Join('\t', match));
            }
        }

        private void OnPriceRetrieved(object sender, PriceRetrievedEventArgs e)
        {
            Console.WriteLine($"{e.Stock.Symbol}: {e.Stock.Price} {e.Stock.Deviation}");
        }
    }
}
