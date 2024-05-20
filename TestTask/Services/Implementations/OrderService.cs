using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<Order> GetOrder()
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: GetOrder was called");
            IQueryable<Order> orders = _context.Orders;
            return orders
                .Where(orders => (orders.Quantity > 1))
                .OrderByDescending(o => o.CreatedAt)
                .FirstAsync();
        }

        public Task<List<Order>> GetOrders()
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: GetOrders was called");
            IQueryable<Order> orders = _context.Orders.Include(o => o.User);
            return orders
                .Where(order => (order.User.Status == Enums.UserStatus.Active))
                .OrderBy(order => order.CreatedAt)
                .Select(p => new Order 
                {
                    Id = p.Id,
                    CreatedAt = p.CreatedAt,
                    Status=p.Status,
                    Price=p.Price,
                    ProductName=p.ProductName,
                    Quantity=p.Quantity,
                    UserId=p.UserId 
                } )
                .ToListAsync();

        }
    }
}
