using ConsoleUI.Models;
using System.Collections.Generic;

namespace ConsoleUI.FileOperations
{
    public interface IFileOperations
    {
        public List<string> ReadCsv(string path);
        public void WriteCsv(List<Stock> stocks, string path);
    }
}
