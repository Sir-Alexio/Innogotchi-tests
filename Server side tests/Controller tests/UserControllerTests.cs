using AutoMapper;
using InnoGotchi_backend.Controllers;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqTestsForApp.Server_side_tests.Controller_tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsersTest()
        {
            //Arrange
            Mock<IUserService> mockService = new Mock<IUserService>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();

            List<User> users = new List<User>() { 
                new User() { UserName = "Alexio"},
                new User(){ UserName = "Anrew"},
                new User(){UserName = "Victoria"}};

            mockService.Setup(x => x.GetAll()).ReturnsAsync(users);

            mockMapper.Setup(x => x.Map<List<UserDto>>(users)).Returns(new List<UserDto>() {
                new UserDto() { UserName = "Alexio"},
                new UserDto(){ UserName = "Anrew"},
                new UserDto(){UserName = "Victoria"}});

            //Act

            UserController controller = new UserController(mockService.Object, mockMapper.Object);
            ObjectResult? result = await controller.GetAllUsers() as ObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            
        }
    }
}
