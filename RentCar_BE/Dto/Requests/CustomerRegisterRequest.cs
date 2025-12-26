using Microsoft.AspNetCore.Mvc;

namespace RentCar_BE.Dto.Requests
{
    public class CustomerRegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber {get; set;}
        public string Address { get; set; }
        public string DriverLicenseNumber { get; set; }

    }
}
