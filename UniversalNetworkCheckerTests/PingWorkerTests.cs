using System;
using System.Net.NetworkInformation;
using Moq;

namespace UniversalNetworkCheckerTests
{
    [TestClass]
    public class PingWorkerTests
    {
		[TestMethod]
		public void PingSend_ReachableIP_ReturnsSuccess()
		{
			var pingWrapperStub = new Mock<IPingWrapper>();
			pingWrapperStub.Setup(i => i.Send("127.0.0.1")).Returns(new PingReplyWrapperStub(true));

            var ping = new PingWorker(pingWrapperStub.Object);

			var reply = ping.DoPing("127.0.0.1");

			Assert.IsTrue(reply.IsPingSuccessful);
		}

        [TestMethod]
        public void PingSend_UnreachableIP_ReturnsSuccess()
        {
            var pingWrapperStub = new Mock<IPingWrapper>();
            pingWrapperStub.Setup(i => i.Send("127.0.0.1")).Returns(new PingReplyWrapperStub(false));

            var ping = new PingWorker(pingWrapperStub.Object);

            var reply = ping.DoPing("127.0.0.1");

            Assert.IsFalse(reply.IsPingSuccessful);
        }
    }

    internal class PingReplyWrapperStub:IPingReplyWrapper
    {
        public bool? IsPingSuccessful { get; set; }

        public PingReply PingReply { get; set; }

        public PingReplyWrapperStub()
        {

        }

        internal PingReplyWrapperStub(bool success)
        {
            IsPingSuccessful = success;
        }

    }
}



