using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Profiles;
using ECommerce.Api.Customers.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using Xunit;

namespace ECommerce.Api.Cutomers.Test
{
    public class CustomersServiceTest
    {
        [Fact]
        public async void GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new CustomersDbContext(options);
            CreateCustomers(dbContext);

            var productsProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new CustomersProvider(dbContext, null, mapper);
            var product = await productsProvider.GetCustomersAsync();
            Assert.True(product.IsSuccess);
            Assert.True(product.Customers.Any());
            Assert.Null(product.ErrorMessage);
        }
        [Fact]
        public async void GetCustomerReturnUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(nameof(GetCustomerReturnUsingInvalidId))
                .Options;
            var dbContext = new CustomersDbContext(options);
            CreateCustomers(dbContext);

            var productsProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new CustomersProvider(dbContext, null, mapper);
            var product = await productsProvider.GetCustomerAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.Customer);
            Assert.NotNull(product.ErrorMessage);
        }
        [Fact]
        public async void GetCustomerReturnUsingValidId()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(nameof(GetCustomerReturnUsingValidId))
                .Options;
            var dbContext = new CustomersDbContext(options);
            CreateCustomers(dbContext);

            var productsProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new CustomersProvider(dbContext, null, mapper);
            var product = await productsProvider.GetCustomerAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Customer);
            Assert.True(product.Customer.Id == 1);
            Assert.Null(product.ErrorMessage);
        }
        private void CreateCustomers(CustomersDbContext dbContext)
        {
            
            for (int i = 1; i < 10; i++)
            {
                dbContext.Customers.Add(new Customer()
                {
                    Id = i,
                    Name = i.ToString(),
                    Address = i.ToString()
                });
                dbContext.SaveChanges();

            }
        }
    }
}

