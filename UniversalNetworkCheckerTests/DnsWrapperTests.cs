using System;
using Moq;

namespace UniversalNetworkCheckerTests
{
	[TestClass]
	public class DnsWrapperTests
	{
		[TestMethod]
		public void IP_RetrieveIsCalled_HostnameIsReturned()
		{
			Mock <DnsWrapper> dnsWrapperMock = new Mock<DnsWrapper>();
			dnsWrapperMock.CallBase = true;
			dnsWrapperMock.Setup(i => i.RetrieveHostNameOfIP("127.0.0.1")).Returns("localhost");

			string hostname = dnsWrapperMock.Object.GetHostEntry("127.0.0.1");

			Assert.AreEqual("localhost", hostname);
		}
	}
}

