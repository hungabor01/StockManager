using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleUI.FileOperations
{
    public class CsvOperationsSettings
    {
        public string CsvDelimiter { get; set; }
    }

    public class CsvOperations : IFileOperations
    {
        private readonly string _delimiter;

        private readonly ILogger<CsvOperations> _logger;

        public CsvOperations(IOptions<CsvOperationsSettings> options, ILogger<CsvOperations> logger)
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

        public void WriteCsv(Dictionary<string, decimal> stocks, string path)
        {
            try
            { 
                using (var writer = new StreamWriter(path))
                {
                    foreach (var stock in stocks)
                    {
                        writer.WriteLine(string.Join(_delimiter, stock.Key, stock.Value.ToString()));
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
