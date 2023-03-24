using SV.Demo.Tests.Customization;

namespace SV.Demo.Tests;

public class AutofixtureTests
{

    [Test]
    public void GivenAutofixture_WhenCreateMany_ThenManyPetersWithSameEmailCreated()
    {
        // Arrange
        var fixture = new Fixture().Customize(new UserCustomization());

        // Act
        var allPeters = fixture.CreateMany<User>().ToList();

        // Assert
        allPeters.Should().AllSatisfy(x => x.Name.Should().Be("Peter"));
    }
    
    [Test]
    public void GivenAutofixture_WhenCreateMany_ThenManyPetersWithSameEmailCreated2()
    {
        // Arrange
        var fixture = new Fixture().Customize(new UserCustomization());

        // Act
        var user = fixture.Create<User>();
        
        // Assert
        user.Should().BePeter();
    }
}
