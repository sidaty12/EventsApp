using EventsApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Appel de la méthode de la classe de base pour configurer les entités d'Identity

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany() // Assurez-vous d'avoir la relation inverse configurée dans votre modèle Event si nécessaire
                .HasForeignKey(r => r.EventId);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Participant)
                .WithMany() // Configurez ici la navigation inverse si vous avez une collection d'inscriptions dans Participant
                .HasForeignKey(r => r.ParticipantId);
        }
    }
}
