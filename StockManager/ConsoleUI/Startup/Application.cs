﻿using ConsoleUI.CommandManager;
using ConsoleUI.EventArgs;
using System;
using System.Threading.Tasks;

namespace StockDataProvider.Startup
{
    public class Application : IApplication
    {
        private readonly ICommandManager _commandManager;

        public Application(ICommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        public async Task Run()
        {
            Console.WriteLine("To search for a symbol of a stock, type: search [symbol]");
            Console.WriteLine("To get the prices to the symbols in the file, type: getprices");
            Console.WriteLine("To get the monthly prices to the symbols in the file, type: monthlyprice");
            Console.WriteLine("To get the prices and deviation to the symbols in the file, type: pad");
            Console.WriteLine("To quit, type: quit");

            string command;
            while((command = Console.ReadLine().ToLower()) != "quit")
            {
                if (command == "getprices")
                {
                    _commandManager.PriceRerieved += OnPriceRetrieved;
                    await _commandManager.GetPrices();
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
                else if (command.Contains("monthlyprice"))
                {
                    var symbol = command.Substring(13);
                    var matches = _commandManager.GetMonthlyPrice(symbol);

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
                else if (command.Contains("pad"))
                {
                    var symbol = command.Substring(4);
                    var matches = _commandManager.GetDailyPriceAndDeviation(symbol);

                    if (matches != null && matches.Count > 0)
                    {
                        Console.WriteLine("price: " + matches[0] + '\t' + "deviation: " + matches[1]);
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

        private void OnPriceRetrieved(object sender, PriceRetrievedEventArgs e)
        {
            Console.WriteLine($"{e.Symbol}: {e.Price}");
        }
    }
}
