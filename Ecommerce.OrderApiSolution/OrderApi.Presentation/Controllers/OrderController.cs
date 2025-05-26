using ecommerce.SharedLibrary.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Mapper;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Domain;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder _orderInterface, IOrderService _orderService) : ControllerBase
    {
      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
           
                var orders = await _orderInterface.GetAllAsync();
                if (!orders.Any())
                    return NotFound("No order detected in the database");

                var (_, orderList) = OrderMapper.FromEntity(null, orders);
                return !orderList!.Any() ? NotFound() : Ok(orderList);
          
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _orderInterface.FindByIdAsync(id);
            if (order == null)
                return NotFound("No Order Found");
            var (_order, _) = OrderMapper.FromEntity(order, null!); 
            return Ok(_order);

        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetClientOrders(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid Data provided");
            var orders = await _orderService.GetOrdersByClientId(clientId);
            return orders.Any() ? Ok(orders) : NotFound("No orders found");

        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetaialsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid data provided");
            var orderDetail = await _orderService.GetOrderDetails(orderId);
            return orderDetail.OrderId > 0 ? Ok(orderDetail) : NotFound();
        }

        [HttpPost]

        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            // Check model state if all data annotation is valid
            if (!ModelState.IsValid)
                return BadRequest("Incomplete data submitted");

            // Convert to Entity
            var order = OrderMapper.ToEntity(orderDTO);
            var response = await _orderInterface.CreateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }



        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDto)
        {
            // Convert from dto to entity
            var order = OrderMapper.ToEntity(orderDto);
            var response = await _orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(Response);
        }



        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            // Convert from dto to entity
            var order = OrderMapper.ToEntity(orderDTO);
            var response = await _orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
