using Moq;
using Xunit;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Pagination;
using ResourceHub.Infrastructure.Services;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Exceptions;

namespace ResourceHub.Tests.Services
{
    public class ResourceServiceTests
    {
        private readonly Mock<IResourceRepository> _resourceRepositoryMock;
        private readonly ResourceService _service;

        public ResourceServiceTests()
        {
            _resourceRepositoryMock = new Mock<IResourceRepository>();

            _service = new ResourceService(
                _resourceRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateResourceAsync_ShouldSaveResource()
        {
            // Arrange
            var resource = new Resource(
                "Meeting Room A",
                "Large meeting room",
                "Floor 1",
                20);

            // Act
            await _service.CreateResourceAsync(resource);

            // Assert
            _resourceRepositoryMock.Verify(
                r => r.AddAsync(resource),
                Times.Once);

            _resourceRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateResourceAsync_ShouldUpdateResource_WhenFound()
        {
            // Arrange
            var existingResource = new Resource(
                "Old Name",
                "Old Description",
                "Old Location",
                10);

            var updatedResource = new Resource(
                "New Name",
                "New Description",
                "New Location",
                20);

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingResource);


            // Act
            await _service.UpdateResourceAsync(
                1,
                updatedResource);


            // Assert
            Assert.Equal("New Name", existingResource.Name);
            Assert.Equal("New Description", existingResource.Description);
            Assert.Equal("New Location", existingResource.Location);
            Assert.Equal(20, existingResource.Capacity);

            _resourceRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateResourceAsync_ShouldThrow_WhenResourceNotFound()
        {
            // Arrange
            var updatedResource = new Resource(
                "New Name",
                "Description",
                "Location",
                20);

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Resource?)null);


            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => _service.UpdateResourceAsync(
                    1,
                    updatedResource));
        }

        [Fact]
        public async Task DeleteResourceAsync_ShouldDeleteResource_WhenFound()
        {

            // Arrange
            var resource = new Resource(
                "Meeting Room",
                "Description",
                "Location",
                10);

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(resource);


            // Act
            await _service.DeleteResourceAsync(1);


            // Assert
            _resourceRepositoryMock.Verify(
                r => r.Delete(resource),
                Times.Once);

            _resourceRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteResourceAsync_ShouldThrow_WhenResourceNotFound()
        {
            // Arrange

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Resource?)null);

            // Act + Assert

            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => _service.DeleteResourceAsync(1));
        }
    }
}
