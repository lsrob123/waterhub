using FluentAssertions;
using WaterHub.Core.Services;
using Xunit;

namespace WaterHub.Core.Tests
{
    public class TextMapServiceTests
    {
        [Fact]
        public void Given_JSON_When_GetMap_Then_ExpectedValuesShouldBeReturned()
        {
            var json = "[{\"key\": \"Home\", \"value\": \"主页\"},{\"key\": \"Health Note\",\"value\": \"健康笔记\", \"context\": \"mycontext\"},{\"key\": \"Edit\",\"value\": \"编辑\"}]";
            var service = new TextMapService(json);

            var value = service.GetMap("home");
            value.Should().Be("主页");

            value = service.GetMap("Health Note");
            value.Should().Be("Health Note");

            value = service.GetMap("Health Note", "mycontext");
            value.Should().Be("健康笔记");
        }
    }
}
