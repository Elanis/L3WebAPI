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

        [HttpGet("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames() {
            return Ok(await _gamesService.GetAllGames());
        }

        [HttpGet("/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Game>> GetById(int id) {

            return Ok(); // 200

            return NoContent(); // 204
        }
    }
}
