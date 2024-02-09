using EventsApp.Models;

namespace EventsApp.Services
{
    public interface IEventsService
    {
        Task<Event> CreateEventAsync(Event @event);
        Task<Event> GetEventByIdAsync(int id);

        Task UpdateEventAsync(Event @event);
        Task DeleteEventAsync(int id);
        Task<List<Event>> GetUpcomingEventsAsync(string? location);
        bool EventExists(int id);
        Task CancelEventAsync(int id);

    }
}
