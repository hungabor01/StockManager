using ConsoleUI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleUI.FileOperations
{
    public class CsvOperationsOptions
    {
        public string CsvDelimiter { get; set; }
    }

    public class CsvOperations : IFileOperations
    {
        private readonly string _delimiter;

        private readonly ILogger<CsvOperations> _logger;

        public CsvOperations(IOptions<CsvOperationsOptions> options, ILogger<CsvOperations> logger)
        {
            _logger = logger;

            if (options == null || options.Value == null || string.IsNullOrWhiteSpace(options.Value.CsvDelimiter))
            {
                throw new ArgumentNullException(nameof(options));
            }
            _delimiter = options.Value.CsvDelimiter;
        }

        public List<string> ReadCsv(string path)
        {
            var symbols = new List<string>();

            try
            {
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(_delimiter);

                        symbols.Add(values[0]);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return symbols;
        }

        public void WriteCsv(List<Stock> stocks, string path)
        {
            try
            { 
                using (var writer = new StreamWriter(path))
                {
                    foreach (var stock in stocks)
                    {
                        writer.WriteLine(string.Join(_delimiter, stock.Symbol, stock.Price.ToString(), stock.Deviation.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
