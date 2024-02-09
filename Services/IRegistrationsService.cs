using EventsApp.Models;

namespace EventsApp.Services
{
    public interface IRegistrationsService
    {
        Task RegisterParticipantAsync(int eventId, int participantId);
        Task CancelRegistrationAsync(int eventId, int participantId);
        Task<List<Participant>> GetParticipantsByEventIdAsync(int eventId);
    }
}
