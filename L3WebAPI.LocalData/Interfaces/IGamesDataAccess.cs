﻿using L3WebAPI.Common.DAO;

namespace L3WebAPI.LocalData.Interfaces {
    public interface IGamesDataAccess {
        IAsyncEnumerable<Game> GetAllGames();
        Task<Game?> GetById(int id);
        Task<IEnumerable<Game>> SearchByName(string name);
        Task Create(Game game);
    }
}
