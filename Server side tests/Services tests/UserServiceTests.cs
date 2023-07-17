using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqTestsForApp.Server_side_tests.Services_tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task UpdateUserTest()
        {
            //Arrange
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

            User user = new User() { Email = "q" };
            UserDto userDto = new UserDto() { Email = "q" };

            UserService userService = new UserService(mockRepository.Object,null,null);
            mockRepository.Setup(x => x.User).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(x => x.GetUserByEmail("q")).ReturnsAsync(user);
            mockUserRepository.Setup(x => x.Update(user)).Returns(Task.CompletedTask);
            mockRepository.Setup(r => r.Save()).Returns(Task.CompletedTask);

            //Act
            bool result = await userService.UpdateUser(userDto);

            //Assert
            Assert.True(result);
        }
    }
}
