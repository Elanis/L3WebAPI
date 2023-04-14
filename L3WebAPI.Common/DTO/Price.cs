namespace L3WebAPI.Common.DTO {
    public class Price {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public static class PriceDAOtoDTOHelper {
        public static DTO.Price ToDto(this DAO.Price originalPrice) {
            return new DTO.Price {
                Value = originalPrice.Value,
                Currency = originalPrice.Currency,
            };
        }

        public static DAO.Price ToDAO(this DTO.Price originalPrice) {
            return new DAO.Price {
                Value = originalPrice.Value,
                Currency = originalPrice.Currency,
            };
        }
    }
}
