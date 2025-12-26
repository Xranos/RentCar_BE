namespace RentCar_BE.Dto.Requests
{
    public class RentalTrRequest
    {
        public string CarId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
