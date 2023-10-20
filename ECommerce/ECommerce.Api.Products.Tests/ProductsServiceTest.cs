using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(options);
            await CreateProductsAsync(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var products = await productsProvider.GetProductsAsync();

            Assert.True(products.IsSuccess);
            Assert.True(products.Products.Any());
            Assert.Empty(products.ErrorMessage);
        }

        private static async Task CreateProductsAsync(ProductsDbContext dbContext)
        {
            await Task.Run(() =>
            {
                if (dbContext.Products.Any())
                    return;
                for (int i = 0; i < 10; i++)
                {
                    var product = new Product() { Name = $"blabla{i}", Inventory = i + 10, Price = (decimal)(i * 2.3) };
                    dbContext.Products.Add(product);
                }
                dbContext.SaveChanges();
            });
        }

        [Fact]
        public async Task GetProductReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(options);
            await CreateProductsAsync(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var productResult = await productsProvider.GetProductAsync(1);

            Assert.True(productResult.IsSuccess);
            Assert.True(productResult.Product.Id == 1);
            Assert.Empty(productResult.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsNullUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(options);
            await CreateProductsAsync(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var productResult = await productsProvider.GetProductAsync(-1);

            Assert.False(productResult.IsSuccess);
            Assert.Null(productResult.Product);
            Assert.NotEmpty(productResult.ErrorMessage);
        }
    }
}