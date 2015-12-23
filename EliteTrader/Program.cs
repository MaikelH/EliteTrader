using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning.Structures;
using EliteTrader.Entities;
using System = EliteTrader.Entities.System;

namespace EliteTrader
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemsLoader systemsLoader = new SystemsLoader("Data\\Systems.csv");
            StationLoader stationsLoaders = new StationLoader("Data\\Stations.csv");

            Dictionary<string, Entities.System> systems = systemsLoader.Load();
            Dictionary<string, List<Station>> stations = stationsLoaders.Load();

            HashSet<String> set = new HashSet<string>();
            systems.Keys.Intersect(stations.Keys).ToList().ForEach(x => set.Add(x));

            Dictionary<string, Entities.System> inhabitedSystems = systems.Where(x => set.Contains(x.Key)).ToDictionary(pair => pair.Key, pair => pair.Value);

            Double[][] coords =
                systems.Select(x => new[] {x.Value.Location.X, x.Value.Location.Y, x.Value.Location.Z})
                    .ToArray();

            KDTree<Entities.System> tree = KDTree.FromData(coords, systems.Values.ToArray());

            KDTreeNodeList<Entities.System> output = tree.Nearest(new[] { 25.0625, -19.96875, 58.875}, 100.0);

            foreach (KDTreeNodeDistance<Entities.System> kdTreeNodeDistance in output)
            {
                Console.WriteLine(kdTreeNodeDistance.Node.Value.Name + ": " + kdTreeNodeDistance.Distance);
            }
        }
    }
}
