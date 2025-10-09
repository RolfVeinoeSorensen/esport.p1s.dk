using Esport.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Esport.Backend.Dtos;
using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public class GamesService(
        DataContext context) : IGamesService
    {

        private readonly DataContext db = context;

        public IEnumerable<Game> GetAllGames()
        {
            return db.Games.Include(gs=>gs.GameServers);
        }

        public Game GetGameById(int id)
        {
            var game = db.Games.Find(id);
            if (game == null) throw new KeyNotFoundException("Game not found");
            return game;
        }
        public GameServer GetGameServerById(int id)
        {
            var gameServer = db.GameServers.Include(g => g.Game).FirstOrDefault(gs => gs.Id.Equals(id));
            if (gameServer == null) throw new KeyNotFoundException("GameServer not found");
            return gameServer;
        }
        public IEnumerable<GameServerDto> GetAllGameServers()
        {
            return db.GameServers.Include(gs => gs.Game).Select(g => new GameServerDto { Game = g.Game, GameServer = g }).ToList();
        }

        public SubmitResponse CreateOrUpdateGame(Game game)
        {
            var existingGame = db.Games.Find(game.Id);
            if (existingGame == null)
            {
                Game newG = new()
                {
                    Description = game.Description,
                    Logo = game.Logo,
                    Name = game.Name
                };
                db.Games.Add(newG);
            }
            else
            {
                existingGame.Name = game.Name;
                existingGame.Description = game.Description;
                db.Games.Update(existingGame);
            }
            db.SaveChanges();
            var response = new SubmitResponse { Ok = true, Message = existingGame == null ? "Spillet blev tilføjet" : "Spillet blev opdateret" };
            return response;
        }

        public SubmitResponse CreateOrUpdateGameServer(GameServer gameServer)
        {
            var existingGameServer = db.GameServers.Find(gameServer.Id);
            if (existingGameServer == null)
            {
                GameServer newGs = new()
                {
                    GameId = gameServer.GameId,
                    Port = gameServer.Port,
                    Server = gameServer.Server
                };
                db.GameServers.Add(newGs);
            }
            else
            {
                existingGameServer.Port = gameServer.Port;
                existingGameServer.Server = gameServer.Server;
                db.GameServers.Update(existingGameServer);
            }
            db.SaveChanges();
            var response = new SubmitResponse { Ok = true, Message = existingGameServer == null ? "Spilserver blev tilføjet" : "spilserver blev opdateret" };
            return response;
        }

        public SubmitResponse RemoveGameServer(int gameServerId)
        {
            var gs = db.GameServers.Find(gameServerId);
            bool isOk = false;
            if (gs != null)
            {
                db.Remove(gs);
                db.SaveChanges();
                isOk = true;
            }
            var response = new SubmitResponse { Ok = isOk, Message = isOk == true ? "Spilserveren blev slettet" : "Spilserveren kunne ikke slettes!" };
            return response;
        }

        public SubmitResponse RemoveGame(int gameId)
        {
            var g = db.Games.Include(gs => gs.GameServers).FirstOrDefault(x => x.Id.Equals(gameId));
            bool isOk = false;
            if (g != null)
            {
                db.RemoveRange(g.GameServers);
                db.SaveChanges();
                db.Remove(g);
            }
            var response = new SubmitResponse { Ok = isOk, Message = isOk == true ? "Spillet og alle tilhørende servere blev slettet" : "Spillet kunne ikke slettes!" };
            return response;
        }
    }
}