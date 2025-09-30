using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAllGames();
        Game GetGameById(int id);
        GameServer GetGameServerById(int id);
        IEnumerable<GameServerDto> GetAllGameServers();
        SubmitResponse CreateOrUpdateGame(Game game);
        SubmitResponse CreateOrUpdateGameServer(GameServer gameServer);
        SubmitResponse RemoveGameServer(int gameServerId);
        SubmitResponse RemoveGame(int gameId);
    }
}
