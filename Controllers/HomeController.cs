using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YahooWebScraperMVC_Auth.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace YahooWebScraperMVC_Auth.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ScrapedData()
        {
            new Scrape();
            SQL SQL = new SQL();
            SqlConnectionStringBuilder builder = SQL.Connection();
            List<Stock> StockList = SQL.RetrieveData(builder, 10);
            return View(from stocks in StockList select stocks);
        }

        public IActionResult ScrapedDataHistory()
        {
            SQL SQL = new SQL();
            SqlConnectionStringBuilder builder = SQL.Connection();
            List<Stock> StockList = SQL.RetrieveData(builder, 50);
            return View(from stocks in StockList select stocks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
