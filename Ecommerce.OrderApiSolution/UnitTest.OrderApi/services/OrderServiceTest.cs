using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Domain;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.OrderApi.Services
{
    public class OrderServiceTest
    {
        private readonly IOrder _orderInterface;
        private readonly IOrderService _orderServiceInterface;

        public OrderServiceTest()
        {
            _orderInterface = A.Fake<IOrder>();
            _orderServiceInterface = A.Fake<IOrderService>();
        }
        private static HttpClient CreateFakeHttpClient(object? responseObj, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var fakeHandler = new FakeHttpMessageHandler(responseObj, statusCode);
            return new HttpClient(fakeHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }

        [Fact]
        public async Task GetMenu_ValidMenuId_ReturnsMenu()
        {
            // Arrange
            int menuId = 1;
            var menuDTO = new MenuDTO(1, "Pizza", 2, 20);
            var httpClient = CreateFakeHttpClient(menuDTO);

            var orderService = new OrderService(null!, httpClient, null!);

            // Act
            var result = await orderService.GetMenu(menuId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(menuId);
            result.Name.Should().Be("Pizza");
        }

        [Fact]
        public async Task GetMenu_InvalidMenuId_ReturnsNull()
        {
            // Arrange
            var httpClient = CreateFakeHttpClient(null, HttpStatusCode.NotFound);
            var orderService = new OrderService(null!, httpClient, null!);

            // Act
            var result = await orderService.GetMenu(999);

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public async Task GetOrdersByClientsId_OrderExist_ReturnOrderDetails()
        {
            int clientId = 1;

            var orders = new List<Order>
            {
                new() {Id = 1, MenuId = 1, ClientId = clientId, PurchaseQuantity = 2, OrderedDate = DateTime.UtcNow},
                new() {Id = 1, MenuId = 2, ClientId = clientId, PurchaseQuantity = 1, OrderedDate = DateTime.UtcNow},
            };

            A.CallTo(() => _orderInterface.GetOrdersAsync(A<Expression<Func<Order, bool>>>.Ignored)).Returns(orders);
            var orderService = new OrderService(_orderInterface, null, null);

           
         


            var result = await orderService.GetOrdersByClientId(clientId);


            result.Should().NotBeNull();
            result.Should().HaveCount(orders.Count);
            result.Should().HaveCountLessThanOrEqualTo(2);


        }

        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public FakeHttpMessageHandler(object? responseObj, HttpStatusCode statusCode)
            {
                if (responseObj is null)
                {
                    _response = new HttpResponseMessage(statusCode);
                }
                else
                {
                    var json = JsonSerializer.Serialize(responseObj);
                    _response = new HttpResponseMessage(statusCode)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                }
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_response);
            }
        }
    }
}

