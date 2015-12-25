using System;
using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Structures;
using EliteTrader.Entities;
using EliteTrader.Exception;
using EliteTrader.Loaders;

namespace EliteTrader
{
    public sealed class EliteTraderApplication
    {
        private readonly string _stationsFile;
        private readonly string _systemsFile;
        private readonly Dictionary<string, Entities.System> _systems;
        private readonly Dictionary<string, List<Station>> _stations;

        private KDTree<Entities.System> _tree;

        public delegate void MessageEventHandler(object sender, EliteTraderEventArgs e);

        public EliteTraderApplication(string stationsFile, string systemsFile)
        {
            _stationsFile = stationsFile;
            _systemsFile = systemsFile;

            _systems = loadSystems(_systemsFile);

            _stations = loadStations(_stationsFile);

            // Initialize kdtree with all known systems.
            Double[][] coords =_systems.Select(x => new[] { x.Value.Location.X, x.Value.Location.Y, x.Value.Location.Z })
                                       .ToArray();
            _tree = KDTree.FromData(coords, _systems.Values.ToArray());
        }

        public void OnMessage(EliteTraderEventArgs e)
        {
            Messaged?.Invoke(this, e);
        }

        /// <summary>
        /// Get the shortest route between two systems. 
        /// </summary>
        /// <param name="from">Start system</param>
        /// <param name="to">End system</param>
        /// <returns></returns>
        public List<Entities.System> GetShortestRoute(Entities.System from, Entities.System to)
        {
            OnMessage(new EliteTraderEventArgs($"Jump distance: {JumpRange} ly"));
            return new List<Entities.System>();
        }

        /// <summary>
        /// Resolve the system from the given input string. The strings dont have to be
        /// match exact. A combination of system and station can also be used to resolve the
        /// system. The system and station should be separated by a forward slash.
        /// 
        /// Example input:
        /// erav    -> Eravate
        /// 
        /// era/ack -> Eravate
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public Entities.System ResolveSystem(string to)
        {
            string searchNameSystem = to;

            if (to.Contains("/"))
            {
                return ResolveSystemStation(to);
            }

            return ResolveSystemName(to);
        }

        /// <summary>
        /// Find the shortest route between two systems. Will use the global jump distance set in the EliteTrader 
        /// application.
        /// 
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>Ordered list of systems. First system in the list is the start system.</returns>
        public List<Entities.System> FindShortestRoute(Entities.System start, Entities.System end)
        {
            return new List<Entities.System>();
        }

        /// <summary>
        /// Find all the system closest to a system.
        /// </summary>
        /// <param name="system"></param>
        /// <param name="distance">Radius to search in</param>
        /// <returns>List of systems.</returns>
        public Dictionary<Entities.System, double> FindClosestSystems(Entities.System system, double distance)
        {
            KDTreeNodeList<Entities.System> closest = _tree.Nearest(new double[] {system.Location.X, system.Location.Y, system.Location.Z}, distance);

            closest.Sort((nodeDistance, treeNodeDistance) => nodeDistance.Distance.CompareTo(treeNodeDistance.Distance));

            return closest.Select(x => new KeyValuePair<Entities.System, double>(x.Node.Value, x.Distance))
                          .Where(x => x.Key.Name != system.Name)  
                          .ToDictionary(pair => pair.Key, pair => pair.Value);
        } 

        #region properties

        public Dictionary<string, Entities.System> Systems => _systems;

        public Dictionary<string, List<Station>> Stations => _stations;

        public event  MessageEventHandler Messaged;

        public double JumpRange { get; set; } = 5.0;

        #endregion

        private Dictionary<string, Entities.System> loadSystems(string systemsStream)
        {
            SystemsLoader loader = new SystemsLoader(systemsStream);

            return loader.Load();
        }

        private Dictionary<string, List<Station>> loadStations(string stationsFile)
        {
            StationLoader loader = new StationLoader(stationsFile);

            return loader.Load();
        }

        private Entities.System ResolveSystemName(string to)
        {
            IEnumerable<Entities.System> matchingSystems = Systems.Where(x => x.Key.Contains(to.ToUpper()))
                                                                  .Select(x => x.Value);

            if (matchingSystems.Count() > 1)
            {
                throw new MultipleSystemException("Found multiple systems for " + to);
            }

            return matchingSystems.First();
        }

        /// <summary>
        /// Resolve the system from a system/station combination.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        private Entities.System ResolveSystemStation(string to)
        {
            string[] splitted = to.Split('/');

            IEnumerable<Entities.System> matchingSystems = Systems.Where(x => x.Key.Contains(splitted[0]))
                                                                    .Where(x => Stations[x.Key].Where(y => y.StationName.Contains(splitted[1])).Count() > 0)
                                                                    .Select(x => x.Value);
            if (matchingSystems.Count() > 0)
            {
                throw new MultipleSystemException("Found multiple systems for input: " + to);
            }
            if (matchingSystems.Count() == 0)
            {
                throw new NoSystemsFoundException("No systems found for input: " + to);
            }

            return matchingSystems.First();
        }
    }
}
