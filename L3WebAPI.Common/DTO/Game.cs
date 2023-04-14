namespace L3WebAPI.Common.DTO {
    public class Game {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUri { get; set; }
        public IEnumerable<Price> Prices { get; set; }
    }

    public static class GameDAOtoDTOHelper {
        public static DTO.Game ToDto(this DAO.Game originalGame) {
            return new DTO.Game() {
                Id = originalGame.Id,
                Name = originalGame.Name,
                Description = originalGame.Description,
                LogoUri = $"https://example.com/img/logos/{originalGame.Id}.png",
                Prices = originalGame.Prices.Select(x => x.ToDto()),
            };
        }
    }
}
