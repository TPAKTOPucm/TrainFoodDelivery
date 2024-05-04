using TrainFoodDelivery.Controllers;
using Xunit;

namespace TrainFoodDelivery.Tests;

public class TestControllers
{
    [Fact]
    public void TestRTDC()
    {
        var controller = new ReadyToDeliverController(null, null, null);
        var actual = controller.Put(22, "test");
        Assert.Equal("22test", actual);
    }

    [Fact]
    public void TestOrderController()
    {
        var controller = new OrderController(null, null, null);
        var actual = controller.Product(0);
        Assert.Equal(controller.BadRequest(), actual.Result);
    }

    [Fact]
    public void TestCookController()
    {
        var controller = new CookController();
        var actual = controller.Get();
        Assert.NotNull(actual);
    }
}
