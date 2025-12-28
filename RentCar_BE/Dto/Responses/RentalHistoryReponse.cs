namespace RentCar_BE.Dto.Responses
{
    public class RentalHistoryReponse
    {
        public string RentalId { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public string CarName { get; set; }
        public decimal PricePerDay { get; set; }

        public int TotalDays { get; set; }
        public decimal TotalPrice { get; set; }

        public bool PaymentStatus { get; set; }
    }
}
