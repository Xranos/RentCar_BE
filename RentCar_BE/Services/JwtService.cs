using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RentCar_BE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentCar_BE.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.CustomerId),
                new Claim(JwtRegisteredClaimNames.Email, customer.Email),
                new Claim(ClaimTypes.Name, customer.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
               issuer: _configuration["JwtSettings:Issuer"],
               audience: _configuration["JwtSettings:Audience"],
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:30"])),
               signingCredentials: credentials
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
