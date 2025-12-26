using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_BE.Data;
using RentCar_BE.Dto.Requests;
using RentCar_BE.Dto.Responses;
using RentCar_BE.Models;
using System.Security.Claims;

namespace RentCar_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : Controller
    {

        private readonly AppDbContext _context;
        public RentalController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        private async Task<string> GenerateIdRentalAsync()
        {
            var last = await _context.Rentals
                .Where(r => r.RentalId.StartsWith("RNT"))
                .OrderByDescending(r => r.RentalId)
                .Select(r => r.CustomerId)
                .FirstOrDefaultAsync();

            var nextNumber = 1;
            if (!string.IsNullOrEmpty(last) && last.Length > 3 && int.TryParse(last.Substring(3), out var n))
                nextNumber = n + 1;

            return $"RNT{nextNumber:000}";
        }

        [HttpPost("rental")]
        public async Task<IActionResult> RentalTr(
            [FromBody] RentalTrRequest request
            )
        {
            if (request.PickupDate >= request.ReturnDate)
                return BadRequest("Pickup date must be before return date!");

            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
                return Unauthorized("Invalid token!");

            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.CarId == request.CarId && c.Status == true);

            if (car == null)
                return NotFound("Car not found!");

            var dateConflict = await _context.Rentals.AnyAsync(r =>
                r.CarId == request.CarId &&
                request.PickupDate < r.ReturnDate &&
                request.ReturnDate > r.RentalDate
            );

            if (dateConflict)
                return BadRequest("Car is already rented in the selected date range!");

            var totalDays = (request.ReturnDate - request.PickupDate).Days;
            var totalPrice = totalDays * car.PricePerDay;

            var rental = new Rental
            {
                RentalId = await GenerateIdRentalAsync(),
                RentalDate = request.PickupDate,
                ReturnDate = request.ReturnDate,
                TotalPrice = totalPrice,
                PaymentStatus = false,

                CustomerId = customerId,
                CarId = request.CarId
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rental created successfully",
                rentalId = rental.RentalId,
                totalPrice
            });
        }

        [HttpGet("history")]
        public async Task<IActionResult> RentalHistory()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
                return Unauthorized("Invalid token!");

            var histories = await _context.Rentals
                .Where(r => r.CustomerId == customerId)
                .Include(r => r.Car)
                .OrderByDescending(r => r.RentalDate)
                .Select(r => new RentalHistoryReponse
                {
                    RentalId = r.RentalId,
                    PickupDate = r.RentalDate,
                    ReturnDate = r.ReturnDate,

                    CarName = $"{r.Car.Name} {r.Car.Year}",
                    PricePerDay = r.Car.PricePerDay,

                    TotalDays = (r.ReturnDate - r.RentalDate).Days,
                    TotalPrice = r.TotalPrice,
                    PaymentStatus = r.PaymentStatus ? "Sudah Dibayar" : "Belum Dibayar"
                })
                .ToListAsync();

            return Ok(histories);
        }

    }
}
