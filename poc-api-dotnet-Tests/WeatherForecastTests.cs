using Microsoft.Extensions.Logging;
using NSubstitute;
using poc_api_dotnet_cicd_template_usage_fase4.Controllers;

namespace poc_api_dotnet_Tests;

public class WeatherForecastTests
{
    [Fact]
    public void WeatherForecastTestsGetShouldReturnFiveItens()
    {
        // Arrange
        var logger = Substitute.For<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger);

        // Act
        var result = controller.Get();
        
        // Assert
        Assert.Equal(5, result.Count());
    }
}