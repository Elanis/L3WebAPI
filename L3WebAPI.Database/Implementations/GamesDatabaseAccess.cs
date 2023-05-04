using L3WebAPI.Common.DAO;
using L3WebAPI.LocalData.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace L3WebAPI.Database.Implementations {
    public class GamesDatabaseAccess : IGamesDataAccess {
        private readonly DatabaseContext _databaseContext;
        public GamesDatabaseAccess(DatabaseContext databaseContext) {
            _databaseContext = databaseContext;
        }

        public async Task Create(Game game) {
            _databaseContext.games.Add(game);
            await _databaseContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<Game> GetAllGames() {
            return _databaseContext.games.AsAsyncEnumerable();
        }

        public async Task<Game?> GetById(int id) {
            return await _databaseContext.games.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Game>> SearchByName(string name) {
            return _databaseContext.games.Where(x => x.Name.Contains(name));
        }
    }
}
