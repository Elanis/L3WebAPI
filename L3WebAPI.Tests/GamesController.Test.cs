using FluentAssertions;
using L3WebAPI.Common.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net;
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
            int id = 1;
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
    }
}
