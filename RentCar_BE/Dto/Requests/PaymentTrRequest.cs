namespace RentCar_BE.Dto.Requests
{
    public class PaymentTrRequest
    {
        public string RentalId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
