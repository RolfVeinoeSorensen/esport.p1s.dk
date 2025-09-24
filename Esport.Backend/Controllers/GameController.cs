using Esport.Backend.Authorization;
using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
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
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<GameServerDto>> GetAllGameServers()
        {
            var gameServers = gamesService.GetAllGameServers();
            return Ok(gameServers);
        }
    }
}