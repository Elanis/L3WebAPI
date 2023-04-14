using L3WebAPI.Common.DTO;

namespace L3WebAPI.Business.Interfaces {
    public interface IGamesService {
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game?> GetById(int id);
    }
}
