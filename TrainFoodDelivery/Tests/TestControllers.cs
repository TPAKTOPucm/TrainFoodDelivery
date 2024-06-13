using TrainFoodDelivery.Controllers;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.Repository;
using TrainFoodDelivery.Services;
using Xunit;

namespace TrainFoodDelivery.Tests;

public class TestControllers
{
    [Fact]
    public void TestUnauthRTDC()
    {
        //Arrange
        var controller = new ReadyToDeliverController
            (
                new NullCache(),
                null,
                new ControllerUtils(new NullCache(), null)
            );
        //Act
        var actual = controller.Order("not", 4);
        //Assert
        Assert.Equal(controller.Forbid(), actual.Result);
    }

    [Fact]
    public void TestOrderController()
    {
        var controller = new OrderController
            (
                null,
                new NullCache(),
                new ControllerUtils(new NullCache(), null)
            );
        var actual = controller.Product(0);
        Assert.Equal(controller.BadRequest(), actual.Result);
    }

    [Fact]
    public void TestCookController()
    {
        var controller = new CookController
            (
                null,
                new NullCache(),
                new ControllerUtils(new NullCache(), null)
            );
        var actual = controller.GetRecipes("ff",0);
        Assert.Equal(controller.Forbid(), actual.Result);
    }
}
