using Microsoft.EntityFrameworkCore;
using RentCar_BE.Models;
namespace RentCar_BE.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
    }
}
