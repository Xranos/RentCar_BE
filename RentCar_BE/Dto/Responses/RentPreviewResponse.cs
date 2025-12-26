namespace RentCar_BE.Dto.Responses
{
    public class RentPreviewResponse
    {
        public string CarId { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string Transmission { get; set; }
        public int NumberOfCarSeats { get; set; }
        public decimal PricePerDay { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalDays { get; set; }
        public decimal TotalPrice { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }

        public List<string> ImageLink { get; set; }
    }
}
