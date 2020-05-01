using System.Collections.Generic;

namespace ConsoleUI.FileOperations
{
    public interface IFileOperations
    {
        public List<string> ReadCsv(string path);
        public void WriteCsv(Dictionary<string, decimal> stocks, string path);
    }
}
