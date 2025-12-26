using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_BE.Data;
using RentCar_BE.Dto;
using RentCar_BE.Dto.Requests;
using RentCar_BE.Dto.Responses;

namespace RentCar_BE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {

        private readonly AppDbContext _context;
        public CarController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchCars(
            [FromQuery]DateTime pickupDate,
            [FromQuery]DateTime returnDate,
            int page = 1,
            int pageSize = 10
            )
        {
            if (pickupDate >= returnDate)
                return BadRequest("Pickup date must be before return date!");

            var rentedCarId = _context.Rentals.Where(r =>
                pickupDate < r.ReturnDate &&
                returnDate > r.RentalDate
            ).Select(r => r.CarId);

            var query = _context.Cars
                .Include(c => c.CarImages)
                .Where(c => !rentedCarId.Contains(c.CarId))
                .Select(c => new CarSearchRequest
                {
                    CarId = c.CarId,
                    Name = c.Name,
                    PricePerDay = c.PricePerDay,
                    Images = c.CarImages
                    .Select(i => new CarImageRequest
                    {
                        ImageLink = i.ImageLink
                    })
                    .ToList()
                });

            var totalData = await query.CountAsync();

            var cars = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                totalData,
                totalPage = (int)Math.Ceiling(totalData / (double)pageSize),
                data = cars
            });

        }

        [HttpGet("{carId}/rent-preview")]
        [Authorize]
        public async Task<IActionResult> RentPreview(
            String carId,
            [FromQuery] DateTime pickupDate,
            [FromQuery] DateTime returnDate
            )
        {
            if (pickupDate >= returnDate)
                return BadRequest("Pickup date must be before return date!");

            var car = await _context.Cars
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == carId && c.Status == true);

            if (car == null)
                return NotFound("Car not found");

            var totalDays = (returnDate -  pickupDate).Days;
            var totalPrice = totalDays * car.PricePerDay;

            var CustomerName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var CustomerEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            var response = new RentPreviewResponse
            {
                CarId = car.CarId,
                Model = car.Model,
                Name = car.Name,
                Transmission = car.Transmission,
                NumberOfCarSeats = car.NumberOfCarSeats,
                PricePerDay = car.PricePerDay,

                PickupDate = pickupDate,
                ReturnDate = returnDate,
                TotalDays = totalDays,
                TotalPrice = totalPrice,

                CustomerName = CustomerName,
                CustomerEmail = CustomerEmail,

                ImageLink = car.CarImages
                    .Select(i => i.ImageLink)
                    .ToList()
            };
            return Ok(response);

        }


    }
}
