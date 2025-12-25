using Microsoft.AspNetCore.Mvc;
using RentCar_BE.Models;

namespace RentCar_BE.Services
{
    public interface IJwtService 
    {
        string GenerateToken(Customer customer);
    }
}
