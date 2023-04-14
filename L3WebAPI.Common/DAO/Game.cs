namespace L3WebAPI.Common.DAO {
    public class Game {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Price> Prices { get; set; }
    }
}
