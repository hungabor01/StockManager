﻿using ConsoleUI.EventArgs;
using ConsoleUI.ExtensionMethods;
using ConsoleUI.FileOperations;
using ConsoleUI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockDataServices.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleUI.CommandManager
{
    public class CommandManagerOptions
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
    }

    public class CommandManager : ICommandManager
    {
        public event EventHandler<PriceRetrievedEventArgs> PriceRerieved;

        private readonly string _inputFilePath;
        private readonly string _outputFilePath;

        private readonly ILogger<CommandManager> _logger;
        private readonly IFileOperations _fileOperations;
        private readonly IStockClient _client;

        public CommandManager(IStockClient client, ILogger<CommandManager> logger, IFileOperations fileOperations, IOptions<CommandManagerOptions> options)
        {
            _logger = logger;
            _fileOperations = fileOperations;
            _client = client;

            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.Value.InputFilePath))
            {
                throw new ArgumentNullException(nameof(options.Value.InputFilePath));
            }
            _inputFilePath = options.Value.InputFilePath;

            if (string.IsNullOrWhiteSpace(options.Value.OutputFilePath))
            {
                throw new ArgumentNullException(nameof(options.Value.OutputFilePath));
            }
            _outputFilePath = options.Value.OutputFilePath;
        }

        public async Task GetPricesAndDeviation()
        {
            var symbols = _fileOperations.ReadCsv(_inputFilePath);

            var stocks = new List<Stock>();

            int counter = 0;

            foreach (var symbol in symbols)
            {                
                try
                {
                    var prices = _client.GetPricesAndDeviations(symbol);

                    if (prices?.Count > 0)
                    {   
                        var stock = new Stock
                        {
                            Symbol = symbol,
                            Price = prices.First(),
                            Deviation = prices.GetDeviation()
                        };

                        stocks.Add(stock);

                        PriceRerieved(this, new PriceRetrievedEventArgs(stock));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }

                counter++;
                if (counter % 5 == 0)
                {
                    counter = 0;
                    await Task.Delay(61000);
                }                
            }

            _fileOperations.WriteCsv(stocks, _outputFilePath);
        }

        public List<string[]> Search(string symbol)
        {
            try
            {
                return _client.Search(symbol);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }
    }
}
