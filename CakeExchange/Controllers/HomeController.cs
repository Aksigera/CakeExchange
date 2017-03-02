using System;
using System.Globalization;
using System.Linq;
using CakeExchange.Data;
using CakeExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CakeExchange.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (ExchangeContext dbContext = new ExchangeContext())
            {
                ViewBag.BuyOrders = dbContext.BuyOrders.ToList();
                ViewBag.SellOrders = dbContext.SellOrders.ToList();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Buy(Buy buyOrder)
        {
            using (ExchangeContext dbContext = new ExchangeContext())
            {
                decimal price;
                decimal.TryParse(Request.Form["Price"], NumberStyles.Any, CultureInfo.InvariantCulture, out price);
                buyOrder.Price = price; //TODO: Из браузера поступает только строка с "." разделителем, а CultureInfo указывает на ru-RU

                dbContext.BuyOrders.Add(buyOrder);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Sell(Sell sellOrder)
        {
            using (ExchangeContext dbContext = new ExchangeContext())
            {
                decimal price;
                decimal.TryParse(Request.Form["Price"], NumberStyles.Any, CultureInfo.InvariantCulture, out price);
                sellOrder.Price = price; //TODO: Из браузера поступает только строка с "." разделителем, а CultureInfo указывает на ru-RU

                dbContext.SellOrders.Add(sellOrder);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}