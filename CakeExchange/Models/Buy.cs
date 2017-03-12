using System.Linq;
using CakeExchange.Data;

namespace CakeExchange.Models
{
    public class Buy : Order
    {
        public static IOrderedQueryable<Order> QueryFree(ExchangeContext dbContext)
        {
            return dbContext.BuyOrders
                .Where(o => o.IsActive)
                .OrderByDescending(o => o.Price)
                .ThenBy(o => o.Date);
        }
    }
}