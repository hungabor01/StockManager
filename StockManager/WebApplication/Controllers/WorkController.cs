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
        public IActionResult Work()
        {
            return View();
        }
        public WorkController(IStockClient client)
        {
            this.client = client;
        }

        [HttpGet("{stockName}")]
        public ActionResult Work(string stockName)
        {
            if(stockName == null){
                return View();
            }
            List<string[]> stocks = client.Search(stockName);
           
            foreach (var item in stocks)
            {
                stockSearch = stockSearch.Append(new StockSearch {
                    Symbol = item[0],
                    Region = item[1],
                    Name   = item[2],
                    Currenc= item[3]
                });
            }
            ViewData.Model = stockSearch;

            return View();
        }
    }
}
