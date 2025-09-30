using Esport.Backend.Authorization;
using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Models;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class GamesController(IGamesService gamesService) : ControllerBase
    {
        private readonly IGamesService gamesService = gamesService;

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Game>> GetAllGames()
        {
            var games = gamesService.GetAllGames();
            return Ok(games);
        }
        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]/{id:int}")]
        public ActionResult<Game> GetGameById(int id)
        {
            var game = gamesService.GetGameById(id);
            return Ok(game);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]/{id:int}")]
        public ActionResult<GameServer> GetGameServerById(int id)
        {
            var gameServer = gamesService.GetGameServerById(id);
            return Ok(gameServer);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<GameServerDto>> GetAllGameServers()
        {
            var gameServers = gamesService.GetAllGameServers();
            return Ok(gameServers);
        }
        [Authorize([UserRole.Admin])]
        [HttpPost("[action]")]
        public ActionResult<SubmitResponse> CreateOrUpdateGame([FromBody] Game game)
        {
            var gameResult = gamesService.CreateOrUpdateGame(game);
            return Ok(gameResult);
        }

        [Authorize([UserRole.Admin])]
        [HttpPost("[action]")]
        public ActionResult<SubmitResponse> CreateOrUpdateGameServer([FromBody] GameServer gameServer)
        {
            var gameServers = gamesService.CreateOrUpdateGameServer(gameServer);
            return Ok(gameServers);
        }

        [Authorize([UserRole.Admin])]
        [HttpDelete("[action]")]
        public ActionResult<SubmitResponse> RemoveGameServer(int gameServerId)
        {
            var gameServers = gamesService.RemoveGameServer(gameServerId);
            return Ok(gameServers);
        }

        [Authorize([UserRole.Admin])]
        [HttpDelete("[action]")]
        public ActionResult<SubmitResponse> RemoveGame(int gameId)
        {
            var gameServers = gamesService.RemoveGame(gameId);
            return Ok(gameServers);
        }
    }
}