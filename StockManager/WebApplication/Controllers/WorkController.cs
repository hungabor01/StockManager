using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StockDataServices.DataServices;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Controller]
    [Route("/api/Work/search/")]
    public class WorkController : Controller
    {
        private readonly IStockClient _client;
        private  IEnumerable<StockSearch> _stockSearch = new List<StockSearch>();
        private IEnumerable<GlobalQuote> _globalQuote = new List<GlobalQuote>();

        public WorkController(IStockClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("{stockName}")]
        public ActionResult Index(string stockName)
        {
            if(string.IsNullOrWhiteSpace(stockName)){
                return View();
            }

            var stocks = _client.Search(stockName);
           
            foreach (var item in stocks)
            {
                _stockSearch = _stockSearch.Append(new StockSearch {
                    Symbol = item[0],
                    Name = item[1],
                    Region   = item[2],
                    Currenc= item[3]
                });
            }

            ViewData.Model = _stockSearch;
            return View();
        }

        [HttpGet("StockPriceData")]
        public ActionResult StockPriceData()
        {
            var stockValue = Request.Query["Symbol"].ToString();
            if (string.IsNullOrWhiteSpace(stockValue))
            {
                return View();
            }

            var stock = _client.GetStockData(stockValue);

            _globalQuote = _globalQuote.Append(new GlobalQuote
            {
                Symbol = stock[0],
                Open = decimal.Parse(stock[1]),
                High = decimal.Parse(stock[2]),
                Low = decimal.Parse(stock[3]),
                Price= decimal.Parse(stock[4]),
                Volume = decimal.Parse(stock[5]),
                LatestTradingDay= stock[6],
                PreviousClose = decimal.Parse(stock[7]),
                Change = decimal.Parse(stock[8]),
                ChangePercent = stock[9]
            });

            ViewData.Model = _globalQuote;
            return View();
        }
    }
}
