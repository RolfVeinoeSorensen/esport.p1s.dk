using Esport.Backend.Dtos;
using Esport.Backend.Entities;

namespace Esport.Backend.Services
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAllGames();
        Game GetGameById(int id);
        IEnumerable<GameServerDto> GetAllGameServers();
    }
}
