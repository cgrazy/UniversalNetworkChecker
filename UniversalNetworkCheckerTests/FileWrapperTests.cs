using System.Reflection;

namespace UniversalNetworkCheckerTests;

[TestClass]
public class FileWrapperTests
{
    [TestMethod]
    public void FileExists_ReturnsTrue()
    {
        var a = new FileWrapper();

        Assert.IsTrue(a.Exists(Assembly.GetExecutingAssembly().Location));
    }

    [TestMethod]
    public void FileExists_ReturnsFalse()
    {
        var a = new FileWrapper();

        Assert.IsFalse(a.Exists("C:\\temp\\notexisting.file"));
    }
}
