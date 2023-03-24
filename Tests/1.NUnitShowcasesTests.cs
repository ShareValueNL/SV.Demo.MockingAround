namespace Tests;

[TestFixture]
public class NUnitShowcasesTests
{
    private List<int> _numbers;

    [SetUp]
    public void SetUp()
    {
        _numbers = new List<int> { 1, 2, 3, 4, 5 };
    }

    [TearDown]
    public void TearDown()
    {
        _numbers = null;
    }

    [Test]
    public void TestAdd()
    {
        _numbers.Add(6);
        Assert.That(_numbers, Has.Member(6));
    }

    [Test]
    public void TestRemove()
    {
        _numbers.Remove(5);
        Assert.That(_numbers, Has.No.Member(5));
    }

    [Test]
    public void TestException()
    {
        Assert.Throws<ArgumentException>(() => {
            throw new ArgumentException();
        });
    }

    [TestCase(12, 3, 4)]
    [TestCase(12, 2, 6)]
    [TestCase(12, 4, 3)]
    public void DivideTest(int n, int d, int q)
    {
        Assert.AreEqual(q, n / d);
    }

    [TestCase(12, 3, ExpectedResult = 4)]
    [TestCase(12, 2, ExpectedResult = 6)]
    [TestCase(12, 4, ExpectedResult = 3)]
    public int DivideTest(int n, int d)
    {
        return (n / d);
    }

    [TestCaseSource(nameof(TestData))]
    public void TestAdditionWithData(int a, int b, int expected)
    {
        var result = a + b;
        Assert.That(result, Is.EqualTo(expected));
    }

    private static readonly object[] TestData =
    {
        new object[] { 1, 2, 3 },
        new object[] { 0, 0, 0 },
        new object[] { -1, 1, 0 }
    };
}