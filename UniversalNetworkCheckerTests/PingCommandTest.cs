using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;


namespace UniversalNetworkCheckerTests
{
    [TestClass]
    public class PingCommandTest
    {
        [TestMethod]
        public async Task Delay_CompletesWithinTwoSeconds()
        {
            // Create instance of internal PingCommand via reflection
            var pingCommandType = typeof(PingCommand);
            var instance = Activator.CreateInstance(pingCommandType, nonPublic: true);
            Assert.IsNotNull(instance, "Failed to create PingCommand instance.");

            // Get the non-public Delay method and invoke it
            var delayMethod = pingCommandType.GetMethod("Delay", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(delayMethod, "Could not find Delay method on PingCommand.");

            var sw = Stopwatch.StartNew();
            var task = (Task)delayMethod.Invoke(instance, null)!;
            await task; // await the private async Task Delay()
            sw.Stop();

            // Delay uses Task.Delay(1000) so it should take approximately 1 second.
            Assert.IsTrue(sw.ElapsedMilliseconds >= 800, $"Delay finished too quickly ({sw.ElapsedMilliseconds}ms).");
            Assert.IsTrue(sw.ElapsedMilliseconds < 2000, $"Delay took too long ({sw.ElapsedMilliseconds}ms).");
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