using TrainFoodDelivery.Controllers;
using TrainFoodDelivery.Controllers.Utils;
using Xunit;

namespace TrainFoodDelivery.Tests;

public class TestControllers
{
    [Fact]
    public void TestRTDC()
    {
        var controller = new ReadyToDeliverController(null, null, new ControllerUtils(null, null));
        var actual = controller.Put(22, "test");
        Assert.Equal("22test", actual);
    }

    [Fact]
    public void TestOrderController()
    {
        var controller = new OrderController(null, null, new ControllerUtils(null, null));
        var actual = controller.Product(0);
        Assert.Equal(controller.BadRequest(), actual.Result);
    }

    [Fact]
    public void TestCookController()
    {
        var controller = new CookController(null, null, new ControllerUtils(null, null));
        var actual = controller.GetRecipes("ff",0);
        Assert.NotNull(actual);
    }
}
