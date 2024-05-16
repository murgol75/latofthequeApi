using lutoftheque.bll.models;
using lutoftheque.Entity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.Tests.Mocks
{
    public static class PlayersMock
    {
        public static List<PlayerParticipateDto> GetPlayers()
        {
            using (var reader = new StreamReader("C:\\Users\\murgo\\source\\repos\\latofthequeApi\\lutoftheque.bll.Tests\\Mocks\\players.json"))
            {
                var json = reader.ReadToEnd();
                var players = JsonConvert.DeserializeObject<List<PlayerParticipateDto>>(json);
                return players;
            }
        }
    }
}