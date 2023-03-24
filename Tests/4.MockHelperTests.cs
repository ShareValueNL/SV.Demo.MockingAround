using Tests.Helpers;

namespace Tests;

public class MockHelperTests
{
    private User _user;
    private UserService _userService;
    private Mock<IApplicationContext> _mockedDbContext;

    [SetUp]
    public void Setup()
    {
        // Do this here to force yourself to call _userService for assertion
        var users = new Fixture().CreateMany<User>().ToList();
        var firstUser = users.First();

        _mockedDbContext = EntityFrameworkMockHelper.GetMockContext(users.AsQueryable(), context => context.Users);

        _user = firstUser;
        _userService = new UserService(new UserRepository(_mockedDbContext.Object));
    }

    [Test]
    public async Task Given_WhenGetByIdAsync_ThenUserShouldBeReturned()
    {
        // Arrange
        var userId = _user.Id;

        // Act
        var result = await _userService.GetUserById(userId);

        // Assert
        result.Should().BeEquivalentTo(_user);
    }

    [Test]
    public async Task GivenUserWithChanges_WhenUpdateUser_ThenUserShouldBeUpdated()
    {
        // Arrange
        var userId = _user.Id;
        _user.Email = "testuser@sv.nl";

        // Act
        await _userService.UpdateUser(_user);

        // Assert
        var assertUser = await _userService.GetUserById(userId);
        assertUser.Email.Should().Be(_user.Email);
    }
    
    [Test]
    public async Task GivenUserId_WhenDeleteUser_ThenUserShouldNotBePresent()
    {
        // Arrange
        var userId = _user.Id;
        
        // Act
        await _userService.DeleteUser(userId);

        // Assert
        var user = await _userService.GetUserById(userId);
        user.Should().BeNull();
    }

    [Test]
    public async Task GivenUser_WhenInsertUser_ThenUserShouldBePresent()
    {
        // Arrange
        User user = new()
        {
            Id = 42,
            Email = "insert@sv.nl",
            Name = "Insert"
        };

        // Act
        await _userService.InsertUser(user);

        // Assert
        var checkUser = await _userService.GetUserById(user.Id);
        checkUser.Should().BeEquivalentTo(user);
    }
}
