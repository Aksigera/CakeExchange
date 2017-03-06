using System.Linq;
using CakeExchange.Data;
using CakeExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeExchange.Controllers
{
    public class HomeController : Controller
    {
        private ExchangeContext _dbContext;

        public HomeController(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewBag.BuyOrders = _dbContext.BuyOrders
                .Where(o => o.IsActive)
                .OrderByDescending(o => o.Price)
                .ToList();

            ViewBag.SellOrders = _dbContext.SellOrders
                .Where(o => o.IsActive)
                .OrderBy(o => o.Price)
                .ToList();

            ViewBag.Transactions = _dbContext.Transactions
                .OrderBy(t => t.Date)
                .Include(t => t.Buy)
                .Include(t => t.Sell)
                .ToList();
            
            return View();
        }

        [HttpPost]
        public IActionResult Buy(Buy buyOrder)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");


            _dbContext.BuyOrders.Add(buyOrder);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Sell(Sell sellOrder)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            _dbContext.SellOrders.Add(sellOrder);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}