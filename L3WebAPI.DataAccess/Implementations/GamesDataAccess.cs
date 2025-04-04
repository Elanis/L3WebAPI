﻿using L3WebAPI.Common;
using L3WebAPI.Common.Dao;
using L3WebAPI.DataAccess.Interfaces;

namespace L3WebAPI.DataAccess.Implementations {
	public class GamesDataAccess : IGamesDataAccess {
		private static List<GameDAO> games = [
			new GameDAO {
				AppId = Guid.NewGuid(),
				Name = "Portal 2",
				Prices = [
					new PriceDAO {
						Valeur = 19.99M,
						Currency = Currency.USD,
					}
				]
			},
			new GameDAO {
				AppId = Guid.NewGuid(),
				Name = "Half-Life 2",
				Prices = [
					new PriceDAO {
						Valeur = 14.99M,
						Currency = Currency.EUR,
					},
					new PriceDAO {
						Valeur = 15.99M,
						Currency = Currency.USD,
					}
				]
			}
		];

		public async Task<IEnumerable<GameDAO>> GetAllGames() {
			return games;
		}

		public async Task<GameDAO?> GetGameById(Guid id) {
			return games.FirstOrDefault(x => x.AppId == id);
		}

		public async Task CreateGame(GameDAO game) {
			games.Add(game);
		}

		public async Task<IEnumerable<GameDAO>> SearchByName(string name) {
			return games.Where(x => x.Name.Contains(
				name,
				StringComparison.OrdinalIgnoreCase
			));
		}

		public async Task UpdateGame(GameDAO game) {
			var gameById = await GetGameById(game.AppId);
			if (gameById != null) {
				games.Remove(gameById);
			}
			games.Add(game);
		}

		public async Task DeleteGame(Guid id) {
			games.Remove(await GetGameById(id));
		}
	}
}
