using lutoftheque.api.Dto;
using lutoftheque.bll.models;

namespace lutoftheque.api.Mappers
{
    public static class GameMapper
    {
        // le mapper model BLL vers DTO
        public static GameLightDto ToDTO (this GameLightDtoBll game) 
        {
            return new GameLightDto
            {
                GameId = game.GameId,
                GameName = game.GameName,
                Picture = game.Picture
            };
        }
    }
}
