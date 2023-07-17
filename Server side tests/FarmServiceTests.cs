using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using InnoGotchi_backend.Models.Entity;

namespace MoqTestsForApp.Server_side_tests
{
    public class FarmServiceTests
    {
        [Fact]
        public async Task UpdateFarmTest()
        {
            // Arrange
            var mockRepository = new Mock<IRepositoryManager>();
            var mockFarmRepository = new Mock<IFarmRepository>(); 
            mockRepository.Setup(r => r.Farm).Returns(mockFarmRepository.Object);

            var farmService = new FarmService(mockRepository.Object);

            Farm farm = new Farm()
            {
                FarmName = "my favourite farm",
                AlivePetsCount = 5,
                DeadPetsCount = 8
            };

            // Arrange the mock behavior for Update
            mockFarmRepository.Setup(r => r.Update(farm)).Returns(Task.CompletedTask);

            // Arrange the mock behavior for Save
            mockRepository.Setup(r => r.Save()).Returns(Task.CompletedTask);

            // Act
            bool result = await farmService.UpdateFarm(farm);

            // Assert
            mockFarmRepository.Verify(r => r.Update(farm), Times.Once);
            mockRepository.Verify(r => r.Save(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateFarmExeptionTest()
        {
            // Arrange
            var mockRepository = new Mock<IRepositoryManager>();
            var mockFarmRepository = new Mock<IFarmRepository>();
            mockRepository.Setup(r => r.Farm).Returns(mockFarmRepository.Object);

            var farmService = new FarmService(mockRepository.Object);

            Farm farm = new Farm()
            {
                FarmName = "my favourite farm",
                AlivePetsCount = 5,
                DeadPetsCount = 8
            };

            // Arrange the mock behavior for Update
            mockFarmRepository.Setup(r => r.Update(farm)).Returns(Task.CompletedTask);

            // Arrange the mock behavior for Save
            mockRepository.Setup(r => r.Save()).Throws(new DbUpdateException());

            // Act
            var exception = await Assert.ThrowsAsync<CustomExeption>(async () => await farmService.UpdateFarm(farm));

            //Assert
            Assert.Equal("Can not update database", exception.Message);
            Assert.Equal(StatusCode.UpdateFailed, exception.StatusCode);

            
        }

    }
}
