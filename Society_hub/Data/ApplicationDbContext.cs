using Microsoft.EntityFrameworkCore;
using Society_hub.Models;

namespace Society_hub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database Tables
        public DbSet<ApartmentBlock> ApartmentBlocks { get; set; }

        public DbSet<Flat> Flats { get; set; }

        public DbSet<Resident> Residents { get; set; }

        public DbSet<Visitor> Visitors { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<MaintenanceBill> MaintenanceBills { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Notice> Notices { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventRegistration> EventRegistrations { get; set; }
    }
}