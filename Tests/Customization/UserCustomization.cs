namespace SV.Demo.Tests.Customization;

public class UserCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<User>(c => c
            .Without(x => x.Id)
            .With(x => x.Name, "Peter"));
    }
}


