using FluentAssertions;
using Klc.Mutabix.Application.Common.Models;

namespace Klc.Mutabix.Tests.Unit.Application.Common;

public class ApiResponseTests
{
    [Fact]
    public void GenericOk_ShouldReturnSuccessWithData()
    {
        var result = ApiResponse<string>.Ok("test-data", "Basarili");

        result.Success.Should().BeTrue();
        result.Data.Should().Be("test-data");
        result.Message.Should().Be("Basarili");
    }

    [Fact]
    public void GenericOk_WithoutMessage_ShouldReturnSuccessWithNullMessage()
    {
        var result = ApiResponse<int>.Ok(42);

        result.Success.Should().BeTrue();
        result.Data.Should().Be(42);
        result.Message.Should().BeNull();
    }

    [Fact]
    public void GenericFail_ShouldReturnFailureWithMessage()
    {
        var result = ApiResponse<string>.Fail("Hata olustu");

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Hata olustu");
        result.Data.Should().BeNull();
    }

    [Fact]
    public void NonGenericOk_ShouldReturnSuccess()
    {
        var result = ApiResponse.Ok("Islem basarili");

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Islem basarili");
    }

    [Fact]
    public void NonGenericOk_WithoutMessage_ShouldReturnSuccess()
    {
        var result = ApiResponse.Ok();

        result.Success.Should().BeTrue();
        result.Message.Should().BeNull();
    }

    [Fact]
    public void NonGenericFail_ShouldReturnFailure()
    {
        var result = ApiResponse.Fail("Bir hata olustu");

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Bir hata olustu");
    }
}
