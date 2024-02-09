using EventsApp.Data;
using EventsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Services
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly AppDbContext _context;

        public RegistrationsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterParticipantAsync(int eventId, int participantId)
        {
            var registration = new Registration { EventId = eventId, ParticipantId = participantId, IsCanceled = false };
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task CancelRegistrationAsync(int eventId, int participantId)
        {
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.EventId == eventId && r.ParticipantId == participantId);
            if (registration != null)
            {
                registration.IsCanceled = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.Registrations
                .Where(r => r.EventId == eventId && !r.IsCanceled)
                .Select(r => r.Participant)
                .ToListAsync();
        }
    }

}
