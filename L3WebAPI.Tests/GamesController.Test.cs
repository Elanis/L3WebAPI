using FluentAssertions;
using L3WebAPI.Common.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Text;
using System.Text.Json;

namespace L3WebAPI.Tests {
    public class GamesControllerTest {
        public HttpClient client { get; }
        public TestServer server { get; }

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() {
            PropertyNameCaseInsensitive = true,
        };

        public GamesControllerTest() {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            client = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async void ShouldGet200_GET_AllGames() {
            var response = await client.GetAsync("/api/Games/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = JsonSerializer.Deserialize<IEnumerable<Game>>(
                await response.Content.ReadAsStringAsync(),
                jsonOptions
            );

            data.Should().NotBeEmpty();
        }

        [Fact]
        public async void ShouldGet204_GET_ByInvalidId() {
            int id = 10000000;
            var response = await client.GetAsync($"/api/Games/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(70)]
        [InlineData(400)]
        public async void ShouldGet200_GET_BySpecificId(int id) {
            var response = await client.GetAsync($"/api/Games/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = JsonSerializer.Deserialize<Game>(
                await response.Content.ReadAsStringAsync(),
                jsonOptions
            );

            data.Should().NotBeNull();
            data.Id.Should().NotBe(0);
            data.Name.Should().NotBeNull();
        }

        [Theory]
        [InlineData("z", 0)] // None
        [InlineData("r", 1)] // Portal
        [InlineData("a", 2)] // Portal + Half Life
        public async void ShouldGet200_Valid_Count_GET_SeachByName(string name, int size) {
            var response = await client.GetAsync($"/api/Games/search/{name}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = JsonSerializer.Deserialize<IEnumerable<Game>>(
                await response.Content.ReadAsStringAsync(),
                jsonOptions
            );

            data.Count().Should().Be(size);
        }

        [Fact]
        public async void Should201_ValidEntity_POST_Create() {
            var game = new Game {
                Id = 1,
                Name = "test",
                Description = "test",
                Prices = new List<Price> {
                    new Price {
                        Currency = Common.Currency.EUR,
                        Value = 42
                    }
                },
                LogoUri = ""
            };

            var content = new StringContent(
                JsonSerializer.Serialize(game),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Games/", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void Should400_InvalidName_POST_Create(string name) {
            var game = new Game {
                Id = 1,
                Name = name,
                Description = "test",
                Prices = new List<Price> {
                    new Price {
                        Currency = Common.Currency.EUR,
                        Value = 42
                    }
                },
                LogoUri = ""
            };

            var content = new StringContent(
                JsonSerializer.Serialize(game),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Games/", content);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            if (name is not null) {
                (await response.Content.ReadAsStringAsync())
                    .Should().Be("Name is empty !");
            }
        }

        [Fact]
        public async void Should201_DuplicatePrices_POST_Create() {
            var game = new Game {
                Id = 1,
                Name = "test",
                Description = "test",
                Prices = new List<Price> {
                    new Price {
                        Currency = Common.Currency.EUR,
                        Value = 42
                    },
                    new Price {
                        Currency = Common.Currency.EUR,
                        Value = 42
                    }
                },
                LogoUri = ""
            };

            var content = new StringContent(
                JsonSerializer.Serialize(game),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Games/", content);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            (await response.Content.ReadAsStringAsync())
                .Should().Be("Duplicate currencies in prices list !");
        }

        [Fact]
        public async void Should201_NoPrices_POST_Create() {
            var game = new Game {
                Id = 1,
                Name = "test",
                Description = "test",
                Prices = new List<Price> {
                },
                LogoUri = ""
            };

            var content = new StringContent(
                JsonSerializer.Serialize(game),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Games/", content);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            (await response.Content.ReadAsStringAsync())
                .Should().Be("The game doesn't have any price !");
        }
    }
}
