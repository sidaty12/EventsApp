using EventsApp.Models;
using EventsApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event @event)
        {
            var createdEvent = await _eventsService.CreateEventAsync(@event);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _eventsService.GetEventByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest();
            }

            try
            {
                if (!_eventsService.EventExists(id))
                {
                    return NotFound();
                }

                await _eventsService.UpdateEventAsync(updatedEvent);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gérer l'exception de mise à jour concurrentielle ici
            }

            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelEvent(int id)
        {
            try
            {
                await _eventsService.CancelEventAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetUpcomingEvents(string? location)
        {
            var events = await _eventsService.GetUpcomingEventsAsync(location);
            return Ok(events);
        }

    }
}
