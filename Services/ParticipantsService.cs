using EventsApp.Data;
using EventsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Services
{
    public class ParticipantsService : IParticipantsService
    {
        private readonly AppDbContext _context;

        public ParticipantsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Participant> AddParticipantAsync(Participant participant)
        {
            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
            return participant;
        }

        public async Task<List<Event>> GetParticipantEventsAsync(int participantId)
        {
            return await _context.Registrations
                .Where(r => r.ParticipantId == participantId && !r.IsCanceled)
                .Select(r => r.Event)
                .ToListAsync();
        }
    }

}
