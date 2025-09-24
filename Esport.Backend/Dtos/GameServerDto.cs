using Esport.Backend.Entities;

namespace Esport.Backend.Dtos
{
    public class GameServerDto
    {
        public required Game Game {  get; set; }
        public required GameServer GameServer { get; set; }
    }
}
