using System.ComponentModel.DataAnnotations;

namespace EventsApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        public bool IsCanceled { get; set; }
    }
}
