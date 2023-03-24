namespace SV.Demo.Tests.Customization;

public class UserAssertions
{
    private readonly User _user;

    public UserAssertions(User user)
    {
        _user = user;
    }

    [CustomAssertion]
    public void BePeter(string because = "", params object[] becauseArgs) => _user.Name.Should().Be("Peter");

    public void BeEquivalentTo(User user)
    {
        _user.Name.Should().Be(user.Name);
        _user.Email.Should().Be(user.Email);
    }

    public void BeNull() => _user.Should().BeNull();
}

public static class UserAssertionExtensions
{
    public static UserAssertions Should(this User user)
    {
        return new UserAssertions(user);
    }
}
