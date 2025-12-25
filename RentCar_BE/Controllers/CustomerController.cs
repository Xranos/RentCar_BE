using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_BE.Data;
using RentCar_BE.Dto;
using RentCar_BE.Models;

namespace RentCar_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        private async Task<string> GenerateIdCustomerAsync()
        {
            var last = await _context.Customers
                .Where(u => u.CustomerId.StartsWith("CUS"))
                .OrderByDescending(u => u.CustomerId)
                .Select(u => u.CustomerId)
                .FirstOrDefaultAsync();

            var nextNumber = 1;
            if (!string.IsNullOrEmpty(last) && last.Length > 3 && int.TryParse(last.Substring(3), out var n))
                nextNumber = n + 1;

            return $"CUS{nextNumber:000}";
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] CustomerRegisterDto request
            )
        {
            var exists = await _context.Customers
                .AnyAsync(c => c.Email == request.Email);

            if (exists)
            {
                return BadRequest("Email already registered!");
            }

            var customer = new Customer
            {
                CustomerId = await GenerateIdCustomerAsync(),
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                DriverLicenseNumber = request.DriverLicenseNumber,
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok("Register Successful");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            string email,
            string password
            )
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.Password == password);

            if (customer == null)
            {
                return Unauthorized("Customer Doesn't Exists!");
            }

            return Ok("Login Successful");
        }

    }
}
