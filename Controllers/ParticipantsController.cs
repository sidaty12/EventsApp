using EventsApp.Models;
using EventsApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantsService _participantsService;

        public ParticipantsController(IParticipantsService participantsService)
        {
            _participantsService = participantsService;
        }

        [HttpPost]
        public async Task<ActionResult<Participant>> AddParticipant([FromBody] Participant participant)
        {
            var addedParticipant = await _participantsService.AddParticipantAsync(participant);
            return Ok(addedParticipant); // Simplement retourner le participant ajouté
        }


        [HttpGet("{participantId}/events")]
        public async Task<ActionResult<List<Event>>> GetParticipantEvents(int participantId)
        {
            var events = await _participantsService.GetParticipantEventsAsync(participantId);
            return Ok(events);
        }

        // Méthode GetParticipant non implémentée ici. Assurez-vous d'ajouter une implémentation si nécessaire.
    }

}
