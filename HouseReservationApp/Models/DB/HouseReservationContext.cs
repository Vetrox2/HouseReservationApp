using HouseReservationApp.Models.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseReservationApp.Models.DB
{
    public class HouseReservationContext(DbContextOptions<HouseReservationContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.BankAccount)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Houses)
                .WithOne(h => h.Owner)
                .HasForeignKey(h => h.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<House>()
                .HasMany(h => h.Reservations)
                .WithOne(r => r.House)
                .HasForeignKey(r => r.HouseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
