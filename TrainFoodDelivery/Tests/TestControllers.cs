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
        /*var controller = new ReadyToDeliverController(new NullCache(),
            new OrderRepositoryMock(),
            new ControllerUtils(new NullCache(), new TicketRepositoryMock());
        //Act
        var actual = controller.Order("not", 4);
        //Assert
        Assert.Equal(controller.Forbid(), actual.Result);*/
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
        Assert.Equal(controller.Forbid(), actual.Result);
    }
}
