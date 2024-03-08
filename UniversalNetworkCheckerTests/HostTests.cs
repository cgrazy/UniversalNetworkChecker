using System;
namespace UniversalNetworkCheckerTests
{
	[TestClass]
	public class HostTests
	{
		[TestMethod]
		public void ctor_valid_host_ip_success()
		{
			var host = new Host() { Hostname = "abc", IP = "1.2.3.4" };
		}

        [TestMethod]
		[ExpectedException(typeof(ArgumentException))]
        public void ctor_valid_host_invalid_ip_throws_argumentexception()
        {
            var host = new Host() { Hostname = "abc", IP = "1.2.3.a" };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ctor_valid_host_invalid_ip2_throws_argumentexception()
        {
            var host = new Host() { Hostname = "abc", IP = "300.2.3.10" };
        }
    }
}

