using System.Collections.Generic;
using System.IO;
using System.Linq;
using EliteTrader.Entities;
using EliteTrader.Loaders;
using System = EliteTrader.Entities.System;

namespace EliteTrader
{
    public sealed class EliteTraderApplication
    {
        private readonly string _stationsFile;
        private readonly string _systemsFile;
        private readonly Dictionary<string, Entities.System> _systems;
        private readonly Dictionary<string, List<Station>> _stations;

        public delegate void MessageEventHandler(object sender, EliteTraderEventArgs e);

        public EliteTraderApplication(string stationsFile, string systemsFile)
        {
            _stationsFile = stationsFile;
            _systemsFile = systemsFile;

            _systems = loadSystems(_systemsFile);

            _stations = loadStations(_stationsFile);
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
        /// era     -> Eravate
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
                throw new MultipleSystemException("Found multiple systems for" + to);
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
                throw new MultipleSystemException("Found multiple systems for" + to);
            }

            return matchingSystems.First();
        }
    }
}
