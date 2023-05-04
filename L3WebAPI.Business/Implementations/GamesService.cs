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

        public async Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken) {
            try {
                List<Game> games = new List<Game>();
                await foreach (var game in _gamesDataAccess.GetAllGames()) {
                    cancellationToken.ThrowIfCancellationRequested();
                    games.Add(game.ToDto());
                }

                return games;
            } catch (Exception e) {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);

                throw;
            }
        }

        public async Task<Game?> GetById(int id) {
            try {
                var data = await _gamesDataAccess.GetById(id);
                /*if (data is not null) {
                    return data.ToDto();
                } else {
                    return null;
                }*/
                return data?.ToDto();
            } catch (Exception e) {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);

                throw;
            }
        }

        public async Task<IEnumerable<Game>> SearchByName(string name) {
            try {
                return (await _gamesDataAccess.SearchByName(name)).Select(x => x.ToDto());
            } catch (Exception e) {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);

                throw;
            }
        }

        public async Task Create(Game game) {
            if (game == null) {
                throw new ArgumentException("Game object is invalid !");
            }

            if (string.IsNullOrWhiteSpace(game.Name)) {
                throw new ArgumentException("Name is empty !");
            }

            if (!game.Prices.Any()) {
                throw new ArgumentException("The game doesn't have any price !");
            }

            if (game.Prices.DistinctBy(x => x.Currency).Count() != game.Prices.Count()) {
                throw new ArgumentException("Duplicate currencies in prices list !");
            }

            try {
                await _gamesDataAccess.Create(game.ToDAO());
            } catch (Exception e) {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);

                throw;
            }
        }
    }
}
