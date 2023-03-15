using System;
using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repository
{
	public class OrderRepository : IOrderRepository
	{
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
		public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
		{
            _dbContext = dbContext;
		}

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            try
            {
                await using var _db = new ApplicationDbContext(_dbContext);
                await _db.orderHeaders.AddAsync(orderHeader);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            try
            {

                await using var _db = new ApplicationDbContext(_dbContext);
                var orderHeaderFromDb = await _db.orderHeaders.FirstOrDefaultAsync(i => i.OrderHeaderId == orderHeaderId);

                if (orderHeaderFromDb is not null)
                {
                    orderHeaderFromDb.PaymentStatus = paid;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

