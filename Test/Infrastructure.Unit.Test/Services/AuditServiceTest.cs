using Domain.Dto;
using Infrastructure.Services;
using Moq;
using Nest;

namespace Infrastructure.Unit.Test.Services
{
    public class AuditServiceTests
    {
        [Fact]
        public async Task AuditAsync_Should_Call_ElasticClient_IndexDocumentAsync()
        {
            // Arrange
            var mockElasticClient = new Mock<IElasticClient>();
            var service = new AuditService(mockElasticClient.Object);
            var entity = new AuditLogDto();

            // Act
            await service.AuditAsync(entity);

            // Assert
            mockElasticClient.Verify(x => x.IndexDocumentAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
