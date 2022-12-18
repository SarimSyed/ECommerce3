using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using Xunit;

namespace ECommerce.Api.Products.Test
{
    public class ProductsServiceTest
    {
        [Fact]
        public async void GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productsProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductsAsync();
            Assert.True(product.IsSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }
        [Fact]
        public async void GetProductReturnUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnUsingInvalidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productsProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }
        [Fact]
        public async void GetProductReturnUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnUsingValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productsProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id== 1);
            Assert.Null(product.ErrorMessage);
        }
        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i < 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Inventory = i + 10,
                    Price= (decimal) (i * 3.14),
                });
                dbContext.SaveChanges();

            }
        }
    }
}
