using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext , IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("EVENT");
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Sectors)
                    .WithOne(s => s.Event)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.ToTable("SECTOR");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                entity.HasMany(e => e.Seats)
                    .WithOne(s => s.Sector)
                    .HasForeignKey(s => s.SectorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("SEAT");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Version).IsRowVersion();

                entity.HasOne(e => e.CurrentReservation)
                    .WithOne(r => r.Seat)
                    .HasForeignKey<Reservation>(r => r.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Reservations)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.AuditLogs)
                    .WithOne(a => a.User)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.SetNull); 
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("RESERVATION");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AUDIT_LOG");
                entity.HasKey(e => e.Id);
            });
        }
    }
}