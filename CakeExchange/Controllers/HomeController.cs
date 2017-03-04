using System.Linq;
using CakeExchange.Data;
using CakeExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeExchange.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
//            var a = ModelState;
            using (ExchangeContext dbContext = new ExchangeContext())
            {
                ViewBag.BuyOrders = dbContext.BuyOrders
                    .Where(o => o.IsActive)
                    .OrderByDescending(o => o.Price)
                    .ToList();

                ViewBag.SellOrders = dbContext.SellOrders
                    .Where(o => o.IsActive)
                    .OrderBy(o => o.Price)
                    .ToList();

                ViewBag.Transactions = dbContext.Transactions
                    .OrderBy(t => t.Date)
                    .Include(t => t.Buy)
                    .Include(t => t.Sell)
                    .ToList();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Buy(Buy buyOrder)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            using (ExchangeContext dbContext = new ExchangeContext())
            {
                dbContext.BuyOrders.Add(buyOrder);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Sell(Sell sellOrder)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            using (ExchangeContext dbContext = new ExchangeContext())
            {
                dbContext.SellOrders.Add(sellOrder);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}