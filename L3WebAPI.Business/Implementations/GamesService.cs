using L3WebAPI.Business.Interfaces;
using L3WebAPI.Common.DTO;
using L3WebAPI.LocalData.Interfaces;
using Microsoft.Extensions.Logging;

namespace L3WebAPI.Business.Implementations {
    public class GamesService : IGamesService {
        private readonly IGamesDataAccess _gamesDataAccess;
        private readonly ILogger<GamesService> _logger;
        public GamesService(IGamesDataAccess gamesDataAccess, ILogger<GamesService> logger) {
            _gamesDataAccess = gamesDataAccess;
            _logger = logger;
        }

        public async Task<IEnumerable<Game>> GetAllGames() {
            try {
                return (await _gamesDataAccess.GetAllGames()).Select(x => x.ToDto());
            } catch (Exception e) {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);

                throw;
            }
        }
    }
}
