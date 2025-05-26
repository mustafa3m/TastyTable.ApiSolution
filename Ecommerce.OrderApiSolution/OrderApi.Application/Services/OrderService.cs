 using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Mapper;
using OrderApi.Application.Interfaces;
using OrderApi.Domain;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface,HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {


        // GET Menu
        public async Task<MenuDTO> GetMenu(int menuid)
        {
            // Call Product API using Httpclient
            // Redirect this  call to the API Gateway since product Api is not response to outsiders
            var getProduct = await httpClient.GetAsync($"/api/menu/{menuid}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;
            var product = await getProduct.Content.ReadFromJsonAsync<MenuDTO>();
            return product!;
        }


        // GET USER
        public async Task<AppUserDTO> GetUser(int userId)
        {
            // Call User API using Httpclient
            // Redirect this  call to the API Gateway since product Api is not response to outsiders
            var getUser = await httpClient.GetAsync($"/api/user/{userId}");
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }


        // GET ORDER DEATILS BY ID
        public async Task<OrderDetaialsDTO> GetOrderDetails(int clientId)
        {
            // Prepare Order
            var order = await orderInterface.FindByIdAsync(clientId);
            if(order is null)
                return null!;

            // Get Retry Pipeline
            var retrypipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // Prepare Product
            var productDTO = await retrypipeline.ExecuteAsync(async token => await GetMenu(order.MenuId));

            // Prepare Client 
            var AppUserDTO = await retrypipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            // Populate order details 
            return new OrderDetaialsDTO(
                order.Id,
                productDTO.Id,
                AppUserDTO.Id,
                AppUserDTO.Name,
                AppUserDTO.Email,
                AppUserDTO.Address,
                AppUserDTO.PhoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderedDate
                );
        }

        // GET ORDER BY CLIENT ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            // Get all client's orders
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if(!orders.Any()) return null!;

            var (_, _orders) = OrderMapper.FromEntity(null, orders);
            return _orders!;
        }
    }
}
