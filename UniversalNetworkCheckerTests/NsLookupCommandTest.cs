using System.Net.Sockets;
using System.Reflection;
using Moq;

namespace UniversalNetworkCheckerTests
{
    [TestClass]
    public class NsLookupCommandTest2
    {
        [TestMethod]
        public async Task Execute_WhenHostPresent_InvokesDnsAndOutputsMapping()
        {
            var dnsWrapperStub = new Mock<IDnsWrapper>();
            dnsWrapperStub.Setup(i => i.GetHostEntry("1.1.1.1")).Returns("A");
            dnsWrapperStub.CallBase = true;

            List<Host> hostsToCheck = new List<Host>
            {
                new Host { Hostname = "A", IP = "1.1.1.1" }
            };

            // Arrange
            var nsLookupCommandTestStub = new Mock<NsLookupCommand>(new List<string>(), dnsWrapperStub.Object, hostsToCheck);
            nsLookupCommandTestStub.Setup(i => i.IsNsLookupCommandInitialized()).Returns(true);

            await nsLookupCommandTestStub.Object.DoExecution();

            Assert.AreEqual("A", nsLookupCommandTestStub.Object.Hostname);
        }

        [TestMethod]
        public async Task Execute_WhenHostLookupThrows_EmptyIsReturned()
        {
            var dnsWrapperStub = new Mock<IDnsWrapper>();
            dnsWrapperStub.Setup(i => i.GetHostEntry(It.IsAny<string>())).Throws(new SocketException());
            dnsWrapperStub.CallBase = true;

            List<Host> hostsToCheck = new List<Host>
            {
                new Host { Hostname = "A", IP = "1.1.1.1" }
            };

            // Arrange
            var nsLookupCommandTestStub = new Mock<NsLookupCommand>(new List<string>(), dnsWrapperStub.Object, hostsToCheck);
            nsLookupCommandTestStub.Setup(i => i.IsNsLookupCommandInitialized()).Returns(true);

            await nsLookupCommandTestStub.Object.DoExecution();

            Assert.AreEqual(string.Empty, nsLookupCommandTestStub.Object.Hostname);
        }

    }



}