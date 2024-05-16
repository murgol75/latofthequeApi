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
    public static class GamesMock
    {
        public static List<GameDto> GetGames() 
        {
            using (var reader = new StreamReader("C:\\Users\\murgo\\source\\repos\\latofthequeApi\\lutoftheque.bll.Tests\\Mocks\\games.json"))
            { 
                var json = reader.ReadToEnd();
                var games = JsonConvert.DeserializeObject<List<GameDto>>(json);
                return games;
            }
        }
    }
}
