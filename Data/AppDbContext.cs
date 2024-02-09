using EventsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
    }
}
