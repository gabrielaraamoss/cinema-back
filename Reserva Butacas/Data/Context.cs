using Microsoft.EntityFrameworkCore;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Data;

public class Context: DbContext
{
    private readonly IConfiguration _configuration;

    public Context(DbContextOptions<Context> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<DatabaseEntities.CustomerEntity> Customers { get; set; }
    public DbSet<DatabaseEntities.MovieEntity> Movies { get; set; }
    public DbSet<DatabaseEntities.RoomEntity> Rooms { get; set; }
    public DbSet<DatabaseEntities.SeatEntity> Seats { get; set; }
    public DbSet<DatabaseEntities.BillboardEntity> Billboards { get; set; }
    public DbSet<DatabaseEntities.BookingEntity> Bookings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DatabaseEntities.CustomerEntity>().ToTable("Customers");
        modelBuilder.Entity<DatabaseEntities.MovieEntity>().ToTable("Movies");
        modelBuilder.Entity<DatabaseEntities.RoomEntity>().ToTable("Rooms");
        modelBuilder.Entity<DatabaseEntities.SeatEntity>().ToTable("Seats");
        modelBuilder.Entity<DatabaseEntities.BillboardEntity>().ToTable("Billboards");
        modelBuilder.Entity<DatabaseEntities.BookingEntity>().ToTable("Bookings");
    }
}