using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_BE.Data;
using RentCar_BE.Dto.Requests;
using RentCar_BE.Dto.Responses;
using RentCar_BE.Models;
using RentCar_BE.Services;

namespace RentCar_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<Customer> _passwordHasher;

        public CustomerController(AppDbContext appDbContext, IJwtService jwtService, IPasswordHasher<Customer> passwordHasher)
        {
            _context = appDbContext;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
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
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            [FromBody] CustomerRegisterRequest request
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
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                DriverLicenseNumber = request.DriverLicenseNumber,
            };

            customer.Password = _passwordHasher.HashPassword(customer, request.Password);

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok("Register Successful");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] CustomerLoginRequest request
            )
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == request.Email);

            if (customer == null)
            {
                return Unauthorized("Email or Password is wrong!");
            }

            var result = _passwordHasher.VerifyHashedPassword(
                customer,
                customer.Password,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Email or Password is wrong!");
            }

            var token = _jwtService.GenerateToken(customer);

            return Ok(new CustomerLoginResponse
            {
                AccessToken = token,
            });
        }

    }
}
