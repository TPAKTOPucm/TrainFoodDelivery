using TrainFoodDelivery.Controllers;
using Xunit;

namespace TrainFoodDelivery.Tests;

public class TestControllers
{
    [Fact]
    public void TestRTDC()
    {
        var controller = new ReadyToDeliverController(null, null);
        var actual = controller.Put(22, "test");
        Assert.Equal("22test", actual);
    }

    [Theory]
    [InlineData(10,"value10")]
    [InlineData(0,"value0")]
    public void TestOrderController(int value, string expected)
    {
        var controller = new OrderController();
        var actual = controller.Get(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestCookController()
    {
        var controller = new CookController();
        var actual = controller.Get();
        Assert.NotNull(actual);
    }
}
