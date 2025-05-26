using ecommerce.SharedLibrary.logs;
using ecommerce.SharedLibrary.response;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain;
using OrderApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository : IOrder
    {
        private readonly OrderDbContext _db;

        public OrderRepository(OrderDbContext db)
        {
            _db = db;
        }
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = _db.Orders.Add(entity).Entity;
                await _db.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Created order successfully") :
                    new Response(false, "Error occurred while placing order");  
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while Placing  order");
            }

        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            var order = await FindByIdAsync(entity.Id);
            if (order == null)
                return new Response(false, "Order not found");
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return new Response(true, "Order deleted successfully");
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await _db.Orders.FindAsync(id);
                return order != null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await _db.Orders.AsNoTracking().ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occcurred while retrieving order");  
            }
          
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await _db.Orders.Where(predicate).FirstOrDefaultAsync();
                return order is not null ? order : null!;
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retrieving order");
            }
            
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await _db.Orders.Where(predicate).ToListAsync();
                return orders != null ? orders : null!;
            }
            catch(Exception ex)
            {
                LogException.LogExceptions (ex);

                throw new Exception("Error occurred while retrieving orders");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            var order = await FindByIdAsync(entity.Id);
            if (order is null)
                return new Response(false, "Order not found");
            _db.Entry(order).State = EntityState.Detached;
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();
            return new Response(true, "Order updated");
        }
    }
}
