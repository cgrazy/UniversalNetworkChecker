using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UniversalNetworkChecker.Commands.Tests
{




    [TestClass]
    public class NsLookupCommandTest
    {
        // helper to set a property or field (public/protected/private) via reflection
        private void SetMember(object target, string name, object value)
        {
            var t = target.GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var prop = t.GetProperty(name, flags);
            if (prop != null)
            {
                prop.SetValue(target, value);
                return;
            }

            var field = t.GetField(name, flags);
            if (field != null)
            {
                field.SetValue(target, value);
                return;
            }

            throw new InvalidOperationException($"Member '{name}' not found on type {t.FullName}");
        }

        [TestMethod]
        public async Task Execute_WhenHostPresent_InvokesDnsAndOutputsMapping()
        {
            // Arrange
            var cmd = new NsLookupCommand(new List<string>(), new TestDnsWrapper());

            // Mark base.IsInitialized = true
            var baseType = cmd.GetType().BaseType!;
            var isInitMember = baseType.GetProperty("IsInitialized", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (isInitMember != null)
            {
                isInitMember.SetValue(cmd, true);
            }
            else
            {
                var isInitField = baseType.GetField("IsInitialized", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (isInitField != null) isInitField.SetValue(cmd, true);
                else throw new InvalidOperationException("Could not find IsInitialized member on base type.");
            }

            // Capture outputs
            var outputs = new List<string>();
            Action<string> outAct = s => outputs.Add(s);
            // set OutputAction (property or field) on base
            var outProp = baseType.GetProperty("OutputAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (outProp != null) outProp.SetValue(cmd, outAct);
            else
            {
                var outField = baseType.GetField("OutputAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (outField != null) outField.SetValue(cmd, outAct);
                else throw new InvalidOperationException("Could not find OutputAction member on base type.");
            }

            // Create and assign a JFW instance and populate HostsToCheck with an item having IP = "1.2.3.4"
            var jfwProp = baseType.GetProperty("JFW", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (jfwProp == null) throw new InvalidOperationException("Could not find JFW property on base type.");
            var jfwType = jfwProp.PropertyType;
            var jfwInstance = Activator.CreateInstance(jfwType) ?? throw new InvalidOperationException("Could not create JFW instance.");

            // HostsToCheck property on JFW
            var hostsProp = jfwType.GetProperty("HostsToCheck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (hostsProp == null) throw new InvalidOperationException("Could not find HostsToCheck on JFW type.");
            var hostsListType = hostsProp.PropertyType;
            if (!hostsListType.IsGenericType) throw new InvalidOperationException("HostsToCheck is expected to be a generic collection.");

            var elementType = hostsListType.GetGenericArguments()[0];
            var listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            // create host element and set its IP property
            var hostElement = Activator.CreateInstance(elementType) ?? throw new InvalidOperationException("Could not create host element instance.");
            var ipProp = elementType.GetProperty("IP", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (ipProp == null) throw new InvalidOperationException("Host element does not have IP property.");
            ipProp.SetValue(hostElement, "1.2.3.4");

            // add to list
            var addMethod = listInstance!.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public)!;
            addMethod.Invoke(listInstance, new[] { hostElement });

            // assign HostsToCheck and JFW
            hostsProp.SetValue(jfwInstance, listInstance);
            jfwProp.SetValue(cmd, jfwInstance);

            // Act
            await cmd.Execute();

            // Assert - expect mapping line for the IP with hostname returned by TestDnsWrapper
            AssertContains(outputs, (value) => value.Contains("1.2.3.4 => host-for-1.2.3.4"), "a");
        }

        [TestMethod]
        public async Task Execute_WhenDnsThrows_OutputsErrorAndEmptyHostnameMapping()
        {
            // Arrange
            var cmd = new NsLookupCommand(new List<string>(), new ThrowingDnsWrapper());

            // Mark base.IsInitialized = true
            var baseType = cmd.GetType().BaseType!;
            var isInitMember = baseType.GetProperty("IsInitialized", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (isInitMember != null)
            {
                isInitMember.SetValue(cmd, true);
            }
            else
            {
                var isInitField = baseType.GetField("IsInitialized", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (isInitField != null) isInitField.SetValue(cmd, true);
                else throw new InvalidOperationException("Could not find IsInitialized member on base type.");
            }

            // Capture outputs
            var outputs = new List<string>();
            Action<string> outAct = s => outputs.Add(s);
            var outProp = baseType.GetProperty("OutputAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (outProp != null) outProp.SetValue(cmd, outAct);
            else
            {
                var outField = baseType.GetField("OutputAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (outField != null) outField.SetValue(cmd, outAct);
                else throw new InvalidOperationException("Could not find OutputAction member on base type.");
            }

            // Setup JFW with one host "1.2.3.4"
            var jfwProp = baseType.GetProperty("JFW", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (jfwProp == null) throw new InvalidOperationException("Could not find JFW property on base type.");
            var jfwType = jfwProp.PropertyType;
            var jfwInstance = Activator.CreateInstance(jfwType) ?? throw new InvalidOperationException("Could not create JFW instance.");
            var hostsProp = jfwType.GetProperty("HostsToCheck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (hostsProp == null) throw new InvalidOperationException("Could not find HostsToCheck on JFW type.");
            var hostsListType = hostsProp.PropertyType;
            var elementType = hostsListType.GetGenericArguments()[0];
            var listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            var hostElement = Activator.CreateInstance(elementType) ?? throw new InvalidOperationException("Could not create host element instance.");
            var ipProp = elementType.GetProperty("IP", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (ipProp == null) throw new InvalidOperationException("Host element does not have IP property.");
            ipProp.SetValue(hostElement, "1.2.3.4");
            var addMethod = listInstance!.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public)!;
            addMethod.Invoke(listInstance, new[] { hostElement });
            hostsProp.SetValue(jfwInstance, listInstance);
            jfwProp.SetValue(cmd, jfwInstance);

            // Act
            await cmd.Execute();

            // Assert - when DNS throws, RetrieveHostnameFromIP writes an "Unable to retrieve hostname..." line and the exception message,
            // and the final mapping includes the IP with an empty hostname (=> ).

            AssertContains<string>(outputs, (value) => value.Contains("Unable to retrieve hostname for ip 1.2.3.4."), "a");
            AssertContains<string>(outputs, (value) => value.Length > 0, "b"); // there will be an exception message line as well
            AssertContains<string>(outputs, (value) => value.Contains("1.2.3.4 =>"), "c");
        }
        private void AssertContains<T>(IEnumerable<T> collection, Func<T, bool> predicate, string message)
        {
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    return;
                }
            }
            Assert.Fail(message);
        }
    }

    internal class TestDnsWrapper : IDnsWrapper
    {
        public string GetHostEntry(string ip) => $"host-for-{ip}";
    }

    internal class ThrowingDnsWrapper : IDnsWrapper
    {
        public string GetHostEntry(string ip) => throw new SocketException();
    }
}