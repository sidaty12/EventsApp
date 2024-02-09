using EventsApp.Models;

namespace EventsApp.Services
{
    public interface IParticipantsService
    {
        Task<Participant> AddParticipantAsync(Participant participant);
        Task<List<Event>> GetParticipantEventsAsync(int participantId);
    }
}
