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

            Assert.IsTrue(jsonFileWrapper.HostsToCheck?.Count == 2);
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
            jsonFileWrapper.OutputAction = (i)=>new ConsoleOutput().Print(i);
            jsonFileWrapper.LoadJson("abc");

            Assert.IsTrue(jsonFileWrapper.HostsToCheck?.Count == 0);
        }

        [TestMethod]
        public void JsonFileWrapper_IPisEmpty_SetupThrowsException()
        {
            var moqJsonFileReaderStub = new Mock<JsonFileReader>();

            var fileWrapperStub = new Mock<FileWrapper>();
            fileWrapperStub.Setup(x => x.Exists("def")).Returns(true);

            Assert.ThrowsException<ArgumentException>(() => {
                moqJsonFileReaderStub
                       .Setup(i => i.Read("abc"))
                       .Returns(CreateListOfEmptyIP());
                     }); 
        }

        [TestMethod]
        public void JsonFileWrapper_HostnameisEmpty_SetupThrowsException()
        {
            var moqJsonFileReaderStub = new Mock<JsonFileReader>();

            var fileWrapperStub = new Mock<FileWrapper>();
            fileWrapperStub.Setup(x => x.Exists("def")).Returns(true);

            Assert.ThrowsException<ArgumentException>(() => {
                moqJsonFileReaderStub
                       .Setup(i => i.Read("abc"))
                       .Returns(CreateListOfEmptyHostname());
            });
        }

        private List<Host> CreateListOfHost()
        {
           return new List<Host>
           {
               new Host() { Hostname = "host1", IP = "1.2.3.4" },
               new Host() { Hostname = "host2", IP = "1.2.3.5" }
           };
        }


        private List<Host> CreateListOfEmptyIP()
        {
            return new List<Host>
           {
               new Host() { Hostname = "host1", IP = "1.2.3.4" },
               new Host() { Hostname = "host2", IP = "" }
           };
        }

        private List<Host> CreateListOfEmptyHostname()
        {
            return new List<Host>
           {
               new Host() { Hostname = "host1", IP = "1.2.3.4" },
               new Host() { Hostname = "", IP = "1.2.3.4" }
           };
        }
    }
}

