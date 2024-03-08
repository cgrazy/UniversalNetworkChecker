using System;
using Moq;

namespace UniversalNetworkCheckerTests
{
    [TestClass]
    public class JsonFileWrapperTests
	{
        [TestMethod]
        public void JsonFileWrapper_ExistingFile_HostsCreated()
		{
            var moqJsonFileReaderStub = new Mock<JsonFileReader>();

            var fileWrapperStub = new Mock<FileWrapper>();
            fileWrapperStub.Setup(x=>x.Exists("abc")).Returns(true);

            moqJsonFileReaderStub
                .Setup(i => i.Read("abc"))
                .Returns(CreateListOfHost());

            var jsonFileWrapper = new JsonFileWrapper(fileWrapperStub.Object, moqJsonFileReaderStub.Object);
            jsonFileWrapper.LoadJson("abc");

            Assert.IsTrue(jsonFileWrapper.HostsToCheck.Count == 2);
		}

        [TestMethod]
        public void JsonFileWrapper_ExistingFile_HostsToCheckIsEmpty()
        {
            var moqJsonFileReaderStub = new Mock<JsonFileReader>();

            var fileWrapperStub = new Mock<FileWrapper>();
            fileWrapperStub.Setup(x => x.Exists("def")).Returns(true);

            moqJsonFileReaderStub
                .Setup(i => i.Read("abc"))
                .Returns(CreateListOfHost());

            var jsonFileWrapper = new JsonFileWrapper(fileWrapperStub.Object, moqJsonFileReaderStub.Object);
            jsonFileWrapper.LoadJson("abc");


            Assert.IsTrue(jsonFileWrapper.HostsToCheck.Count == 0);
        }

        private List<Host> CreateListOfHost()
        {
           return new List<Host>
           {
               new Host() { Hostname = "host1", IP = "1.2.3.4" },
               new Host() { Hostname = "host2", IP = "1.2.3.5" }
           };
        }
    }
}

