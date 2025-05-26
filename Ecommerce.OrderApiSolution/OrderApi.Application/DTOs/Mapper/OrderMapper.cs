using OrderApi.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Mapper
{
    public static class OrderMapper
    {
        public static Order ToEntity(OrderDTO orderDTO)
        {
            Order order = new Order()
            {
                Id = orderDTO.Id,
                MenuId = orderDTO.MenuId,
                ClientId = orderDTO.ClientId,
                PurchaseQuantity = orderDTO.PurchaseQuantity,
                OrderedDate = orderDTO.OrderedDate,
            };

            return order;
        }


        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order order, IEnumerable<Order> orders)
        {
            if(order is not null)
            {
                var singleOrder = new OrderDTO(order.Id, order.MenuId, order.ClientId, order.PurchaseQuantity, order.OrderedDate);
                return (singleOrder, null);
            }

            if(orders is not null)
            {
                var multipleOrders = orders.Select(o => new OrderDTO(o.Id, o.MenuId, o.ClientId, o.PurchaseQuantity, o.OrderedDate));
                return (null, multipleOrders);
            }

            return (null, null);
        }
    }
}
