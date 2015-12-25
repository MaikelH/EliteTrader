using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace EliteTrader.test
{
    [TestFixture]
    class NavigationTest
    {
        [Test]
        public void TestClosest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            EliteTraderApplication application = new EliteTraderApplication(path + "\\TestData\\Stations.csv", path + "\\TestData\\Systems.csv");

            Entities.System eravate = application.Systems["ERAVATE"];

            Dictionary<Entities.System, double> closestSystems = application.FindClosestSystems(eravate, 25);

            Assert.AreEqual(1, closestSystems.Count);
            Assert.AreEqual("POTRITI", closestSystems.First().Key.Name);
            Assert.AreEqual(5.324985328618286, closestSystems.First().Value);
        }
    }
}
