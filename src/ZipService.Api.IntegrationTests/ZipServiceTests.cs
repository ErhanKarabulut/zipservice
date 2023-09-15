using Microsoft.AspNetCore.Mvc.Testing;

using System.Net;

namespace ZipService.Api.IntegrationTests
{
    public class ZipServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public ZipServiceTests(WebApplicationFactory<Program> factory)
        {
            factory.Server.AllowSynchronousIO = true;
            _httpClient = factory.CreateClient();
        }


        [Fact]
        public async Task GetZipFileWithZeroFiles_ReturnsOk()
        {
            var response = await _httpClient.GetAsync("/0");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await _httpClient.GetAsync("/v2/0");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetZipFileWithTenFiles_ReturnsOk()
        {
            var response = await _httpClient.GetAsync("/10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await _httpClient.GetAsync("/v2/10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetZipFile_ContentLength_ShouldBeValid()
        {
            var response = await _httpClient.GetAsync("/10");
            Assert.True(response.Content.Headers.ContentLength > 0);

            response = await _httpClient.GetAsync("/v2/0");
            Assert.True(response.Content.Headers.ContentLength > 0);
        }
    }
}