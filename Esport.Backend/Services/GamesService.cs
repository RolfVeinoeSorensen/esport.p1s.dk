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
            return db.GameServers.Include(gs => gs.Game).Select(g => new GameServerDto { Game = g.Game, GameServer = g }).ToList();
        }

        public Game CreateOrUpdateGame(Game game)
        {
            var existingGame = db.Games.Find(game.Id);
            if (existingGame == null)
            {
                db.Games.Add(game);
            }
            else
            {
                existingGame.Name = game.Name;
                existingGame.Description = game.Description;
                db.Games.Update(existingGame);
            }
            db.SaveChanges();
            return game;
        }
        
        public GameServer CreateOrUpdateGameServer(GameServer gameServer)
        {
            var existingGameServer = db.GameServers.Find(gameServer.Id);
            if (existingGameServer == null)
            {
                db.GameServers.Add(gameServer);
            }
            else
            {
                existingGameServer.Port = gameServer.Port;
                existingGameServer.Server = gameServer.Server;
                db.GameServers.Update(existingGameServer);
            }
            db.SaveChanges();
            return gameServer;
        }
    }
}