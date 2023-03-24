using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using SV.Demo.Tests.Customization;

namespace Tests;

public class UserRepositoryTests
{
    private IFixture _fixture;
    private int _userId;
    private User _user;
    private UserService _userService;
    private DbContextMock<ApplicationContext> _dbContextMock;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();

        // Do this here to force yourself to call _userService for assertion
        var users = _fixture.CreateMany<User>().ToList();
        var firstUser = users.First();
        _dbContextMock = new DbContextMock<ApplicationContext>();
        _dbContextMock.CreateDbSetMock(x => x.Users, users);

        _userId = firstUser.Id;
        _user = firstUser;
        _userService = new UserService(new UserRepository(_dbContextMock.Object));
    }

    [Test]
    public async Task GivenUserId_WhenGetByIdAsync_ThenUserShouldBeReturned()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var user = _fixture.Create<User>();
        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        var service = new UserService(mockRepository.Object);

        // Act
        var result = await service.GetUserById(1);

        // Assert
        result.Should().BeEquivalentTo(user);
        mockRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Test]
    public async Task Given_WhenGetByIdAsyncWithManualDatabaseMocking_ThenUserShouldBeReturned()
    {
        // Arrange
        var users = _fixture.CreateMany<User>().ToList().AsQueryable();
        var firstUser = users.First();

        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());
        mockSet.Setup(m => m.FindAsync(firstUser.Id)).ReturnsAsync(firstUser);

        var mockDbContext = new Mock<IApplicationContext>();
        mockDbContext.Setup(c => c.Users).Returns(mockSet.Object);

        var service = new UserService(new UserRepository(mockDbContext.Object));

        // Act
        var result = await service.GetUserById(firstUser.Id);

        // Assert
        result.Should().BeEquivalentTo(firstUser);

        // Verify FindAsync has been called and not the synchonous Find
        mockSet.Verify(x => x.FindAsync(firstUser.Id), Times.Once);
        mockSet.Verify(x => x.Find(firstUser.Id), Times.Never);
    }

    [Test]
    public async Task Given_WhenGetByIdAsyncWithDatabaseMocking_ThenUserShouldBeReturned()
    {
        // Act
        var result = await _userService.GetUserById(_userId);

        // Assert
        result.Should().BeEquivalentTo(_user);
    }

    [Test]
    public async Task GivenUserWithChanges_WhenUpdateUser_ThenUserShouldBeUpdated()
    {
        // Arrange
        _user.Email = "testuser@sv.nl";

        // Act
        await _userService.UpdateUser(_user);

        // Assert
        var assertUser = await _userService.GetUserById(_userId);
        assertUser.Email.Should().Be(_user.Email);

        _dbContextMock.Verify(x => x.Users.Update(_user), Times.Once);
    }
    

    [Test]
    public async Task GivenUserId_WhenDeleteUser_ThenUserShouldNotBePresent()
    {
        // Act
        await _userService.DeleteUser(_userId);

        // Assert
        var user = await _userService.GetUserById(_userId);
        user.Should().BeNull();
    }
}