using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<User> GetUser()
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Get user was called");
            IQueryable<Order> orders = _context.Orders.Include(o => o.User);
            return orders
                .Where(orders => (orders.CreatedAt.Year == 2003) && (orders.Status == Enums.OrderStatus.Delivered))
                .GroupBy(o => o.User)
                .Select(o => new
                {
                    User = o.Key,
                    Orders = o.Sum(p => p.Price * p.Quantity)
                })
                .OrderByDescending(p=>p.Orders)
                .Select(p=>p.User)
                .FirstAsync();
        }

        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
