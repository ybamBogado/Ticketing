using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext 
    {
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity => 
            {
                entity.ToTable("AUDIT_LOG");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.User)
                    .WithMany(u => u.AuditLogs)
                    .HasForeignKey(e => e.UserId);

            })







            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints here if needed
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Reservations)
                .HasForeignKey(r => r.EventId);
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Sector)
                .WithMany(se => se.Seats)
                .HasForeignKey(s => s.SectorId);
        }
    }
}
