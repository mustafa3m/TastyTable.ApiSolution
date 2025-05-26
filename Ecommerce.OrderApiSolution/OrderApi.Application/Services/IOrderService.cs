using OrderApi.Application.DTOs;
using OrderApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<OrderDetaialsDTO> GetOrderDetails(int orderId);
        Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId);
        Task<AppUserDTO> GetUser (int userId);
        Task<MenuDTO> GetMenu (int menuId);

    }
}
