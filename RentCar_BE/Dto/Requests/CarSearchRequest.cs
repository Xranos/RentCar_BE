namespace RentCar_BE.Dto.Requests
{
    public class CarSearchRequest
    {
        public string CarId { get; set; }
        public string Name { get; set; }

        public int Year { get; set; }
        public decimal PricePerDay { get; set; }

        public List<CarImageRequest> Images { get; set; }
    }
}
