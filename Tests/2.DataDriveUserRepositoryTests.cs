using EntityFrameworkCoreMock;

namespace SV.Demo.Tests;

public class DataDriveUserRepositoryTests
{
    private UserService _userService;
    private static List<User> _users = new Fixture().CreateMany<User>().ToList();

    [SetUp]
    public void Setup()
    {
        // Do this here to force yourself to call _userService for assertion
        var mockDbContext = new DbContextMock<ApplicationContext>();
        mockDbContext.CreateDbSetMock(x => x.Users, _users);
        
        _userService = new UserService(new UserRepository(mockDbContext.Object));
    }

    [TestCaseSource(nameof(GetDataForTest))]
    public async Task DynamicDataTestMethod(int userId, User expected)
    {
        // Act
        var result = await _userService.GetUserById(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    static IEnumerable<object[]> GetDataForTest()
    {
        yield return new object[] { _users.First().Id, _users.First()};
        yield return new object[] { _users[1].Id, _users[1] };
        yield return new object[] { _users[2].Id, _users[2] };
    }

}
