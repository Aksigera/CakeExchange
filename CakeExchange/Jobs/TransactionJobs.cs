using CakeExchange.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeExchange.Jobs
{
    public static class TransactionJobs
    {
        public static void ConfirmTransaction()
        {
            using (DbContext dbContext = new ExchangeContext())
            {
//                dbContext.Transactions
            }
        }
    }
}