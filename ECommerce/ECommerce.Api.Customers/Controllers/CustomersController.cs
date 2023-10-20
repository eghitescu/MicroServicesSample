
using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomersProvider provider;

        public CustomersController(ICustomersProvider provider)
        {
            this.provider = provider;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await provider.GetCustomersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Customers);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await provider.GetCustomerAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Customer);
            }
            return NotFound();
        }
    }
}
