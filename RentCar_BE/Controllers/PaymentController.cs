using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_BE.Data;
using RentCar_BE.Dto.Requests;
using RentCar_BE.Models;
using System.Security.Claims;

namespace RentCar_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;
        public PaymentController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        private async Task<string> GenerateIdPaymentAsync()
        {
            var last = await _context.Payments
                .Where(p => p.PaymentId.StartsWith("PAY"))
                .OrderByDescending(p => p.PaymentId)
                .Select(p => p.PaymentId)
                .FirstOrDefaultAsync();

            var nextNumber = 1;
            if (!string.IsNullOrEmpty(last) && last.Length > 3 && int.TryParse(last.Substring(3), out var n))
                nextNumber = n + 1;

            return $"PAY{nextNumber:000}";
        }


        [HttpPost("payment")]
        [Authorize]
        public async Task<IActionResult> PaymentTr (
            [FromBody] PaymentTrRequest request
            )
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            var rental = await _context.Rentals
                .FirstOrDefaultAsync(r => r.RentalId == request.RentalId && r.CustomerId == customerId);

            if (rental == null)
                return NotFound("Rental not found!");

            if (rental.PaymentStatus)
                return BadRequest("Rental already paid");

            var payAmount = rental.TotalPrice;

            var payment = new Payment
            {
                PaymentId = await GenerateIdPaymentAsync(),
                PaymentDate = DateTime.Now,
                Amount = payAmount,
                PaymentMethod = request.PaymentMethod,
                RentalId = request.RentalId
            };

            rental.PaymentStatus = true;

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment successful",
                paymentId = payment.PaymentId
            });
        }

    }
}
