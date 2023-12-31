using lutoftheque.Entity.Models;

namespace lutoftheque.api.Dto
{
    public class EventLightDto
    {
        public int EventId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
    public class EventFullDto
    {
        public int EventId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string>? ParticipatingPlayers { get; set; }
        public List<string>? AvailableGames { get; set; }
    }
    public class EventToCreateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int FkOrganizerId { get; set; }
    }

}
