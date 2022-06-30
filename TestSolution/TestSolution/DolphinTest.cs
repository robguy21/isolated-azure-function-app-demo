using System;
using System.Net.Http;
using System.Threading.Tasks;
using Assert = Xunit.Assert;
using Xunit.Abstractions;

namespace IsolatedAzureFunctionAppDemo.TestSolution.TestSolution
{
    public class DolphinTest : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _http;

        public DolphinTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            this._http = new HttpClient();
        }

        [Fact]
        public async Task DolphinRoute_is_accessible_when_queried()
        {
            // Arrange
            var baseUrl =  Environment.GetEnvironmentVariable("BASE_URL");

            var requestUri = $"{baseUrl}/api/dolphin/1/1";

            // Act
            var response = await _http.GetStringAsync(requestUri).ConfigureAwait(false);

            // Assert
            Assert.NotNull(response);
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
