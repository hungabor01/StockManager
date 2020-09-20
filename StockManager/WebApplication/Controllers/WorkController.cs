using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockDataServices.DataServices;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Controller]
    [Route("/api/Work/search/")]
    public class WorkController : Controller
    {
        private readonly IStockClient client;
        private  IEnumerable<StockSearch> stockSearch = new List<StockSearch>();
        private IEnumerable<GlobalQuote> globalQuote = new List<GlobalQuote>();
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public WorkController(IStockClient client)
        {
            this.client = client;
        }

        [HttpGet("{stockName}")]
        public ActionResult Index(string stockName)
        {
            if(stockName == null){
                return View();
            }
            List<string[]> stocks = client.Search(stockName);
           
            foreach (var item in stocks)
            {
                stockSearch = stockSearch.Append(new StockSearch {
                    Symbol = item[0],
                    Name = item[1],
                    Region   = item[2],
                    Currenc= item[3]
                });
            }
            ViewData.Model = stockSearch;

            return View();
        }
        [HttpGet("StockPriceData")]
        public ActionResult StockPriceData()
        {
            var stockValue = Request.Query["Symbol"].ToString();
            if (stockValue == null)
            {
                return View();
            }
            List<string[]> stocks = client.GetStockPriceData(stockValue);

            foreach (var item in stocks)
            {
                globalQuote = globalQuote.Append(new GlobalQuote
                {
                            Symbol = item[0],
                            Open = Decimal.Parse(item[1]),
                            High = Decimal.Parse(item[2]),
                            Low = Decimal.Parse(item[3]),
                            Price=Decimal.Parse(item[4]),
                            Volume = Decimal.Parse(item[5]),
                            LatestTradingDay=item[6],
                            PreviousClose = Decimal.Parse(item[7]),
                            Change = Decimal.Parse(item[8]),
                            ChangePercent = item[9]
                });
            }
            ViewData.Model = globalQuote;

            return View();
        }
    }
}
