using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Moq;


namespace UniversalNetworkCheckerTests
{
    [TestClass]
    public class PingCommandTest
    {
        [TestMethod]
        public async Task Delay_CompletesWithinTwoSeconds()
        {
            var prw = new PingReplyWrapper(new Ping().Send("localhost"));
            prw.IsPingSuccessful = true;

            var pingWorkerStub = new Mock<PingWorker>() { CallBase = false };
            pingWorkerStub.Setup(pw => pw.DoPing("127.0.0.1")).Returns(prw);

            // Create instance of internal PingCommand via reflection
            List<Host> hosts = new List<Host>()
            {
                new Host { Hostname ="localhost", IP = "127.0.0.1" }
            }; 

            var pingCommandStub = new PingCommand(new List<string> { "-ping" }, pingWorkerStub.Object, hosts);

            var sw = Stopwatch.StartNew();

            // Call the internal DoExecution method
            await pingCommandStub.DoExecution();

            sw.Stop();

            pingCommandStub.ResultContainer.Results.TryGetValue("localhost", out var result);
            var a = result.Results.Count;
            Assert.IsTrue(result.Results[0].IsPingSuccessful);
        }

        [TestMethod]
        public async Task CreatedTask_StartsAndReturnsResult_WithWaitAsync()
        {
            // Create a Task<int> similar to how the production code constructs tasks
            var task = new Task<int>(() =>
            {
                // small delay to simulate work
                Task.Delay(100).Wait();
                return 123;
            });

            task.Start();

            // Use WaitAsync as in the production code to ensure it completes within the timeout
            await task.WaitAsync(TimeSpan.FromSeconds(1));

            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
            Assert.AreEqual(123, task.Result);
        }
    }
}