using System.Reflection;

namespace UniversalNerworkChecker_uTest;

public class FileWrapperTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void FileExists_ReturnsTrue()
    {
        var a = new FileWrapper();

        Assert.IsTrue(a.Exists(Assembly.GetExecutingAssembly().Location));
    }

    [Test]
    public void FileExists_ReturnsFalse()
    {
        var a = new FileWrapper();

        Assert.IsTrue(a.Exists("C:\\temp\\notexisting.file"));
    }
}
