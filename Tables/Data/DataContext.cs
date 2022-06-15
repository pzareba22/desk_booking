using Microsoft.EntityFrameworkCore;

namespace Tables.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
