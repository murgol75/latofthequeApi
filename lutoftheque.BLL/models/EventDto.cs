using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.models
{

    public class EventFullDto
    {
        public int EventId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string>? ParticipatingPlayers { get; set; }
        public List<string>? AvailableGames { get; set; }
    }
}
