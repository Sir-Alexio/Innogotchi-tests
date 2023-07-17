using AutoMapper;
using InnoGotchi_backend.Controllers;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using System.Security.Claims;
using System.Text.Json;

namespace MoqTests
{
    public class FarmControllerTests
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

            ObjectResult? result = await controller.CreateFarm(dto) as ObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(dto), result.Value);

        }

        [Fact]
        public async Task BadRequestFarmTest()
        {
            //Arrange
            Mock<IFarmService> farmServiceMoke = new Mock<IFarmService>();
            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();

            FarmDto dto = new FarmDto { FarmName = "test farm name", AlivePetsCount = 0, DeadPetsCount = 0 };

            farmServiceMoke.Setup(p => p.CreateFarm(dto, "q")).ReturnsAsync(false);

            userMock.Setup(x => x.FindFirst(ClaimTypes.Email))
            .Returns(new Claim(ClaimTypes.Email, "q"));

            //Act
            FarmsController controller = new FarmsController(farmServiceMoke.Object, null, null, null)
            {
                ControllerContext = new ControllerContext
                { HttpContext = new DefaultHttpContext { User = userMock.Object } }
            };

            ObjectResult? result = await controller.CreateFarm(dto) as ObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        }

        [Fact]
        public async Task GetCurrentFarmTest()
        {
            //Arrange
            var mockFarmService = new Mock<IFarmService>();
            var userMock = new Mock<ClaimsPrincipal>();
            var mockMapper = new Mock<IMapper>();

            Farm farm = new Farm()
            {
                FarmName = "my favourite farm",
                AlivePetsCount = 5,
                DeadPetsCount = 8
            };
            mockMapper.Setup(x => x.Map<FarmDto>(farm))
                .Returns(new FarmDto { FarmName = "my favourite farm", AlivePetsCount = 5, DeadPetsCount = 8 });
            
            mockFarmService.Setup(x => x.GetFarm("q")).ReturnsAsync(farm);
            
            userMock.Setup(x => x.FindFirst(ClaimTypes.Email))
            .Returns(new Claim(ClaimTypes.Email, "q"));

            //Act
            FarmsController controller = new FarmsController(mockFarmService.Object, mockMapper.Object, null, null)
            {
                ControllerContext = new ControllerContext
                { HttpContext = new DefaultHttpContext { User = userMock.Object } }
            };

            var result = await controller.GetCurrentFarm() as ObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

    }
}