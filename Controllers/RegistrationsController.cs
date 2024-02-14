using EventsApp.Models;
using EventsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Participant")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationsService _registrationsService;

        public RegistrationsController(IRegistrationsService registrationsService)
        {
            _registrationsService = registrationsService;
        }

        // Inscrire un participant à un événement
        [HttpPost("{eventId}/participants/{participantId}")]
        public async Task<IActionResult> RegisterParticipant(int eventId, int participantId)
        {
            try
            {
                await _registrationsService.RegisterParticipantAsync(eventId, participantId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Gérer les exceptions (par exemple, événement ou participant non trouvé)
                return BadRequest(ex.Message);
            }
        }

        // Annuler l'inscription d'un participant à un événement 
        [HttpDelete("{eventId}/participants/{participantId}")]
        public async Task<IActionResult> CancelRegistration(int eventId, int participantId)
        {
            try
            {
                await _registrationsService.CancelRegistrationAsync(eventId, participantId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Gérer les exceptions
                return BadRequest(ex.Message);
            }
        }

        // Lister tous les participants d'un événement spécifique
        [HttpGet("{eventId}/participants")]
        public async Task<ActionResult<List<Participant>>> GetParticipants(int eventId)
        {
            var participants = await _registrationsService.GetParticipantsByEventIdAsync(eventId);
            return Ok(participants);
        }
    }

}
