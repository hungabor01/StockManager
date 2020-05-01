using System.Threading.Tasks;

namespace StockDataProvider.Startup
{
    public interface IApplication
    {
        public Task Run();
    }
}
