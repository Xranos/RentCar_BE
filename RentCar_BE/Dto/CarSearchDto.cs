namespace RentCar_BE.Dto
{
    public class CarSearchDto
    {
        public string CarId { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }

        public List<CarImageDto> Images { get; set; }
    }
}
