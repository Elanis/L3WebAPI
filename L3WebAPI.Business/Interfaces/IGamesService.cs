using L3WebAPI.Common.DTO;

namespace L3WebAPI.Business.Interfaces {
    public interface IGamesService {
        Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken);
        Task<Game?> GetById(int id);
        Task<IEnumerable<Game>> SearchByName(string name);
        Task Create(Game game);
    }
}
