using System;
using System.Linq;
using CakeExchange.Common.Settings;
using CakeExchange.Data;
using CakeExchange.Models;
using EntityFrameworkCore.Triggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CakeExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExchangeContext _dbContext;

        public HomeController(ExchangeContext dbContext, IOptions<BackgroundJobsSettings> settings)
        {
            _dbContext = dbContext;
            Triggers<Order>.Inserted += entry => Transaction.Try();
        }

        public IActionResult Index()
        {
            ViewBag.BuyOrders = Models.Buy.QueryFree(_dbContext).ToList();

            ViewBag.SellOrders = Models.Sell.QueryFree(_dbContext).ToList();

            ViewBag.Transactions = _dbContext.Transactions
                .Where(t => t.State == TransactionStates.HasDone)
                .OrderBy(t => t.Date)
                .Include(t => t.Buy)
                .Include(t => t.Sell)
                .ToList();

            return View();
        }

        public IActionResult Admin()
        {
            ViewBag.Transactions = _dbContext.Transactions
                .Where(t => t.State == TransactionStates.NotConfirmedByAdmin)
                .OrderBy(t => t.Date)
                .Include(t => t.Buy)
                .Include(t => t.Sell)
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult SubmitTransaction()
        {
            int id = Int32.Parse(Request.Form.FirstOrDefault(p => p.Key == "transactionId").Value);
            var transaction = _dbContext.Transactions.SingleOrDefault(e => e.Id == id);
            if (transaction != null)
            {
                transaction.State = TransactionStates.NotConfirmedByAdmin;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Admin");
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