using Esport.Backend.Entities;
using Esport.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Esport.Backend.Dtos;

namespace Esport.Backend.Services
{
    public class GamesService(
        DataContext context) : IGamesService
    {

        private readonly DataContext db = context;

        public IEnumerable<Game> GetAllGames()
        {
            return db.Games;
        }

        public Game GetGameById(int id)
        {
            var game = db.Games.Find(id);
            if (game == null) throw new KeyNotFoundException("Game not found");
            return game;
        }
        public IEnumerable<GameServerDto> GetAllGameServers()
        {
            return db.GameServers.Include(gs => gs.Game).Select(g => new GameServerDto{Game=g.Game, GameServer=g}).ToList();
        }
    }
}