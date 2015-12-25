using System;
using EliteTrader.Exception;
using NUnit.Framework;

namespace EliteTrader.test
{
    [TestFixture]
    public class ResolveSystemTest
    {
        [Test]
        public void ResolveSystemExceptionTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            EliteTraderApplication application = new EliteTraderApplication(path + "\\TestData\\Stations.csv", path + "\\TestData\\Systems.csv");

            Assert.Throws<MultipleSystemException>(delegate { application.ResolveSystem("era"); });
        }

        [Test]
        public void ResolveSystemOnlyTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            EliteTraderApplication application = new EliteTraderApplication(path + "\\TestData\\Stations.csv", path + "\\TestData\\Systems.csv");

            Entities.System sys = application.ResolveSystem("eravate");

            Assert.IsNotNull(sys);
            Assert.AreEqual("ERAVATE", sys.Name);
        }

        [Test]
        public void ResolveSystemStationTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            EliteTraderApplication application = new EliteTraderApplication(path + "\\TestData\\Stations.csv", path + "\\TestData\\Systems.csv");

            Entities.System sys = application.ResolveSystem("era/ack");

            Assert.IsNotNull(sys);
            Assert.AreEqual("ERAVATE", sys.Name);
        }
    }
}