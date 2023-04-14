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

        public GamesControllerTest() {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            client = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async void ShouldGet200_GET_Index() {
            var response = await client.GetAsync("/api/Games/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = JsonSerializer.Deserialize<IEnumerable<Game>>(
                await response.Content.ReadAsStringAsync()
            );

            data.Count().Should().BePositive();
        }
    }
}
