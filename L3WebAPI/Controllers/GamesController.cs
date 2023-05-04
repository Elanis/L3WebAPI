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

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames(CancellationToken cancellationToken = default) {
            return Ok(await _gamesService.GetAllGames(cancellationToken));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Game>> GetById(int id) {
            var game = await _gamesService.GetById(id);

            if (game is null) {
                return NoContent();
            }

            return Ok(game);
        }

        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Game>>> SearchByName(string name) {
            return Ok(await _gamesService.SearchByName(name));
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(Game game) {
            try {
                await _gamesService.Create(game);
                return Created($"/api/Games/{game.Id}", game);
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
