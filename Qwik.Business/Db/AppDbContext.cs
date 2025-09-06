using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Qwik.Business
{
    public class AppDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentSettings> AppointmentSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("AppointmentDb");  // For demo; use SQL in prod
        }
    }
}
