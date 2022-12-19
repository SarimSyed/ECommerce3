using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Profiles;
using ECommerce.Api.Orders.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using Xunit;

namespace ECommerce.Api.Orders.Test
{
    public class OrdersApiTest
    {

        [Fact]
        public async void GetOrderReturnUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(nameof(GetOrderReturnUsingInvalidId))
                .Options;
            var dbContext = new OrdersDbContext(options);
            CreateOrders(dbContext);

            var productsProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new OrdersProvider(dbContext, null, mapper);
            var product = await productsProvider.GetOrdersAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.Orders);
            Assert.NotNull(product.ErrorMessage);
        }
        [Fact]
        public async void GetOrderReturnUsingValidId()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(nameof(GetOrderReturnUsingValidId))
                .Options;
            var dbContext = new OrdersDbContext(options);
            CreateOrders(dbContext);

            var productsProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new OrdersProvider(dbContext, null, mapper);
            var product = await productsProvider.GetOrdersAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Orders);
          
            Assert.Null(product.ErrorMessage);
        }
        private void CreateOrders(OrdersDbContext dbContext)
        {
            for (int i = 1; i < 5; i++)
            {
                dbContext.Orders.Add(new Order()
                {
                    Id = i,
                    CustomerId = i,
                    OrderDate= DateTime.Now,
                    Items = new System.Collections.Generic.List<OrderItem> {
                        new OrderItem() { ProductId= i+1 , OrderId = 1, Quantity = i *10, UnitPrice = i*20},
                        new OrderItem() { ProductId= i+1 , OrderId = 1, Quantity = i *10, UnitPrice = i*20},
                        new OrderItem() {  ProductId= i+1 , OrderId = 1, Quantity = i *10, UnitPrice = i*20},
                        new OrderItem() {  ProductId= i+1 , OrderId = 1, Quantity = i *10, UnitPrice = i*20},

                    }
                });
                dbContext.SaveChanges();

            }
        }
    }
}
