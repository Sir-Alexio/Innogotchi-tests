using InnoGotchi_backend.Controllers;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Text.Json;

namespace MoqTests
{
    public class FarmServiceTests
    {
        [Fact]
        public async Task TestCreateNewFarm()
        {
            //Arrange
            Mock<IFarmService> farmServiceMoke = new Mock<IFarmService>();
            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();

            FarmDto dto = new FarmDto {FarmName = "test farm name", AlivePetsCount=0, DeadPetsCount=0};

            farmServiceMoke.Setup(p =>  p.CreateFarm(dto, "q")).ReturnsAsync(true);

            userMock.Setup(x => x.FindFirst(ClaimTypes.Email))
            .Returns(new Claim(ClaimTypes.Email, "q")); 

            //Act
            FarmsController controller = new FarmsController(farmServiceMoke.Object, null, null, null)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                { HttpContext = new DefaultHttpContext { User = userMock.Object } }
            };

            var result = await controller.CreateFarm(dto) as OkObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(dto), result.Value);

        }

    }
}