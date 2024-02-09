using EventsApp.Data;
using EventsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Services
{
    public class EventsService : IEventsService
    {
        private readonly AppDbContext _context;

        public EventsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateEventAsync(Event @event)
        {
            _context.Entry(@event).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Event>> GetUpcomingEventsAsync(string? location)
        {
            IQueryable<Event> query = _context.Events
                .Where(e => e.StartDateTime > DateTime.Now && !e.IsCanceled);

            if (!string.IsNullOrWhiteSpace(location))
            {
                var lowerCaseLocation = location.ToLower();
                query = query.Where(e => e.Location.ToLower().Equals(lowerCaseLocation));
            }

            return await query.ToListAsync();
        }


        public bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        public async Task CancelEventAsync(int id)
        {
            var eventToCancel = await _context.Events.FindAsync(id);
            if (eventToCancel == null) throw new KeyNotFoundException("Event not found.");

            eventToCancel.IsCanceled = true;
            await _context.SaveChangesAsync();
        }

    }

}
