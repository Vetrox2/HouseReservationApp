﻿using HouseReservation.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseReservation.Infrastructure.Data
{
    public class HouseReservationContext(DbContextOptions<HouseReservationContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
    {
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

            modelBuilder.Entity<User>()
               .HasMany(u => u.Reservations)
               .WithOne(r => r.User)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
