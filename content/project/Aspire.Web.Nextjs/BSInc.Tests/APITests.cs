using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AspireTemplate.Tests
{
    public class APITests
    {
        [Fact]
        public async Task GetAPIResourceRootReturnsOkStatusCode()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.BSInc_AppHost>();

            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            // To output logs to the xUnit.net ITestOutputHelper, 
            // consider adding a package from https://www.nuget.org/packages?q=xunit+logging

            await using var app = await appHost.BuildAsync();

            var resourceNotificationService = app.Services
                .GetRequiredService<ResourceNotificationService>();

            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("apiservice");

            await resourceNotificationService.WaitForResourceAsync(
                    "apiservice",
                    KnownResourceStates.Running
                )
                .WaitAsync(TimeSpan.FromSeconds(30));

            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}