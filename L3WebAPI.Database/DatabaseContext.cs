using L3WebAPI.Common;
using L3WebAPI.Common.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace L3WebAPI.Database {
    public class DatabaseContext : DbContext {
        public DbSet<Game> games { get; set; }

        private readonly string ConnectionString;

        public DatabaseContext(IOptions<AppSettings> options) {
            this.ConnectionString = options.Value?.ConnectionString
                    ?? throw new ArgumentNullException(nameof(ConnectionString));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                 => optionsBuilder.UseNpgsql(ConnectionString);

        /* ==
         * protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
            optionsBuilder.UseNpgsql(ConnectionString);
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            var gameBuilder = modelBuilder.Entity<Game>();

            gameBuilder.HasKey(x => x.Id);

            gameBuilder.Property(x => x.Id).HasColumnType("integer");
            gameBuilder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnType("varchar");
            gameBuilder.Property(x => x.Description)
                .HasMaxLength(2000)
                .IsUnicode(true)
                .HasColumnType("varchar");

            gameBuilder.HasIndex(x => x.Name).IsUnique();

            gameBuilder.OwnsMany(x => x.Prices, price => {
                //price.Property(x => x.Value).HasColumnType("decimal(5,2)");
                price.Property(x => x.Value).HasPrecision(5, 2);
                price.Property(x => x.Currency);
            });
        }
    }
}
