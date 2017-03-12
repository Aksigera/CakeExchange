using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CakeExchange.Data;
using CakeExchange.Models;
using Microsoft.Extensions.Configuration;

namespace CakeExchange.Services
{
    public class MakeTransactionService : IDisposable
    {
        private readonly TransactionContext _dbContext;

        public MakeTransactionService(IConfigurationRoot configuration)
        {
            _dbContext = new TransactionContext(configuration);
        }

        public void ProcessTransactions()
        {
            Debug.WriteLine("Transactions processing started...");

            var confirmedTransactions = _dbContext.Transactions
                .Where(o => o.State == TransactionStates.ConfirmedInProgress)
                .ToList();

            Thread.Sleep(10000);

            foreach (Transaction transaction in confirmedTransactions)
            {
                transaction.State = TransactionStates.HasDone;
            }
            _dbContext.SaveChanges();


            Debug.WriteLine("Transactions processing complete.");
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}