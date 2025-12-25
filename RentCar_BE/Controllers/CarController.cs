using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_BE.Data;
using RentCar_BE.Dto;

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
                .Select(c => new CarSearchDto
                {
                    CarId = c.CarId,
                    Name = c.Name,
                    PricePerDay = c.PricePerDay,
                    Images = c.CarImages
                    .Select(i => new CarImageDto
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

        

      


    }
}
