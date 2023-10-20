using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext customersDbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = customersDbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Jessica Smith", Address = "20 Elm St." });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "John Smith", Address = "30 Main St." });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "William Johnson", Address = "100 10th St." });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    return (false, null, "Not found");
                }
                var result = mapper.Map<Models.Customer>(customer);
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers == null || !customers.Any())
                {
                    return (false, null, "Not found");
                }
                var result = mapper.Map<IEnumerable<Db.Customer>,IEnumerable<Models.Customer>>(customers);
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

    }
}
