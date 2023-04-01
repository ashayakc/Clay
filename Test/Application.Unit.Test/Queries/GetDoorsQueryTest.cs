using Application.Common.Interfaces;
using Application.Queries;
using AutoMapper;
using Domain;
using Domain.Dto;
using Moq;
using System.Linq.Expressions;

namespace Application.Unit.Test.Queries
{
    public class GetDoorsQueryHandlerTests
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<RoleDoorMapping>> _roleDoorRepositoryMock;
        private readonly IMapper _mapper;
        private GetDoorsQueryHandler _handler;

        public GetDoorsQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _roleDoorRepositoryMock = new Mock<IRepository<RoleDoorMapping>>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Door, DoorDto>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsDoorList()
        {
            // Arrange
            var userId = 1;
            var roleId = 2;
            var user = new User { Id = userId, RoleId = roleId };
            var door1 = new Door { Id = 1, Name = "Door 1" };
            var door2 = new Door { Id = 2, Name = "Door 2" };
            var roleDoorMapping1 = new RoleDoorMapping { RoleId = roleId, DoorId = door1.Id, Door = door1 };
            var roleDoorMapping2 = new RoleDoorMapping { RoleId = roleId, DoorId = door2.Id, Door = door2 };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(user);
            _roleDoorRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<RoleDoorMapping, bool>>>(), nameof(RoleDoorMapping.Door)))
                .ReturnsAsync(new List<RoleDoorMapping> { roleDoorMapping1, roleDoorMapping2 }.AsQueryable());

            // Act
            _handler = new GetDoorsQueryHandler(_userRepositoryMock.Object, _roleDoorRepositoryMock.Object, _mapper);
            var result = await _handler.Handle(new GetDoorsQuery { UserId = userId }, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(door1.Id, result.First().Id);
            Assert.Equal(door1.Name, result.First().Name);
            Assert.Equal(door2.Id, result.Last().Id);
            Assert.Equal(door2.Name, result.Last().Name);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsEmptyDoorList()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(null as User);

            // Act
            _handler = new GetDoorsQueryHandler(_userRepositoryMock.Object, _roleDoorRepositoryMock.Object, _mapper);
            var result = await _handler.Handle(new GetDoorsQuery { UserId = userId }, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
