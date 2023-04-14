using L3WebAPI.Business.Interfaces;
using L3WebAPI.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace L3WebAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService) {
            _gamesService = gamesService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Game>> GetAllGames() {
            return await _gamesService.GetAllGames();
        }
    }
}
