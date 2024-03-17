using System;
using Moq;

namespace UniversalNetworkCheckerTests
{
	[TestClass]
	public class NsLookupCommandTetss
	{
		[TestMethod]
		public void a()
		{
			var dnsWrapperStub = new Mock<DnsWrapper>();
            dnsWrapperStub.Setup(i => i.GetHostEntry("127.0.0.1")).Returns("localhost");

			NsLookupCommand command = new NsLookupCommand(new List<string> {""}, dnsWrapperStub.Object);
            command.IsInitialized = true;
            command.JFW = new JsonFileWrapperTestable();
            command.JFW.LoadJson("");

			command.Execute();

			Assert.IsTrue(command.Hostname == "localhost", $"Excpected: localhost, but was {command.Hostname}.");
		}
	}

    internal class JsonFileWrapperTestable : IJsonFileWrapper
    {
        public Action<string>? OutputAction { get; set; }
        public List<Host>? HostsToCheck { get; set; }

        public void LoadJson(string fileName)
        {
            HostsToCheck = CreateListOfHost();
        }

        private List<Host> CreateListOfHost()
        {
            return new List<Host>
           {
               new Host() { Hostname = "localhost", IP = "127.0.0.1" }
     	   };
        }
    }
}

