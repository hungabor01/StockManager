﻿using ConsoleUI.FileOperations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockDataServices.DataServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleUI.CommandManager
{
    public class CommandManagerSettings
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
    }

    public class CommandManager : ICommandManager
    {
        private readonly string _inputFilePath;
        private readonly string _outputFilePath;

        private readonly ILogger<CommandManager> _logger;
        private readonly IFileOperations _fileOperations;
        private readonly IStockClient _client;

        public CommandManager(IStockClient client, ILogger<CommandManager> logger, IFileOperations fileOperations, IOptions<CommandManagerSettings> options)
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

        public void GetPrices()
        {
            var symbols = _fileOperations.ReadCsv(_inputFilePath);

            var stocks = new Dictionary<string, decimal>();

            int counter = 0;
            foreach (var symbol in symbols)
            {                
                try
                {
                    var price = _client.GetPrice(symbol);

                    if (price.HasValue)
                    {
                        stocks.Add(symbol, price.Value);
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
                    Task.Delay(61000);
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