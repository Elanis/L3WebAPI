﻿using L3WebAPI.Common;
using L3WebAPI.Common.DAO;
using L3WebAPI.LocalData.Interfaces;

namespace L3WebAPI.LocalData.Implementations {
    public class GamesDataAccess : IGamesDataAccess {
        private static readonly List<Game> _games = new List<Game> {
            new Game {
                Id = 70,
                Name = "Half-Life",
                Description = null,
                Prices = new List<Price> {
                    new Price {
                        Value = 8.19,
                        Currency = Currency.EUR
                    },
                    new Price {
                        Value = 9.99,
                        Currency = Currency.USD
                    },
                }
            },
            new Game {
                Id = 400,
                Name = "Portal",
                Description = "The cake is a lie !",
                Prices = new List<Price> {
                    new Price {
                        Value = 9.75,
                        Currency = Currency.EUR
                    },
                    new Price {
                        Value = 9.99,
                        Currency = Currency.USD
                    },
                }
            },
        };

        public IAsyncEnumerable<Game> GetAllGames() {
            throw new NotImplementedException();
            //return _games;
        }

        public async Task<Game?> GetById(int id) {
            return _games.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Game>> SearchByName(string name) {
            return _games.Where(x => x.Name.Contains(name));
        }

        public async Task Create(Game game) {
            _games.Add(game);
        }
    }
}
