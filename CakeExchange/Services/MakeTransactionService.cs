using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CakeExchange.Services
{
    public class MakeTransactionService : IDisposable
    {
        private readonly DbContext _context;

        public MakeTransactionService(IConfigurationRoot configuration)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            _context = new DbContext(builder.Options);
        }

//            public void CreateTodoItem(TodoItem todoItem)
//            {
//                Console.WriteLine("Run started");
//
//                _context.TodoItems.Add(todoItem);
//                _context.SaveChanges();
//
//                Thread.Sleep(10000);
//
//                Console.WriteLine("Run complete");
//            }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}