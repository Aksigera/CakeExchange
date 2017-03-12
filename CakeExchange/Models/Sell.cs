using System.Linq;
using CakeExchange.Data;

namespace CakeExchange.Models
{
    public class Sell : Order
    {
        public static IOrderedQueryable<Order> QueryFree(ExchangeContext dbContext)
        {
            return dbContext.SellOrders
                .Where(o => o.IsActive)
                .OrderBy(o => o.Price)
                .ThenBy(o => o.Date);
        }
    }
}