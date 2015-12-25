using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EliteTrader.Entities;

namespace EliteTrader.Loaders
{
    public class StationLoader
    {
        private readonly string _filepath;

        public StationLoader(string filepath)
        {
            _filepath = filepath;
        }

        public Dictionary<string, List<Station>> Load()
        {
            Dictionary<string, List<Station>> stations = new Dictionary<string, List<Station>>();

            string[] fileContent = File.ReadAllLines(_filepath);

            fileContent.ToList()
                       .Skip(1)
                       .Select(x => x.Split(','))
                       .Select(x => x.Select(y => y.Replace("'", "")).ToArray())
                       .ToList()
                       .ForEach(x => addStationToDictionary(stations, x));

            return stations;
        }

        private void addStationToDictionary(Dictionary<string, List<Station>> stations, string[] x)
        {
            Station station = new Station(x[0], 
                x[1].ToUpper(), 
                Convert.ToInt32(x[2]), 
                convertToBoolean(x[3]), 
                x[4],
                convertToBoolean(x[5]),
                convertToBoolean(x[6]),
                DateTime.Now,
                convertToBoolean(x[8]),
                convertToBoolean(x[9]),
                convertToBoolean(x[10]),
                convertToBoolean(x[11]),
                convertToBoolean(x[12]));

            if (stations.ContainsKey(station.Systemname))
            {
                List<Station> stationList;
                stations.TryGetValue(station.Systemname, out stationList);
                stationList.Add(station);
            }
            else
            {
                stations.Add(station.Systemname, new List<Station> {station});
            }
        }

        private bool convertToBoolean(string x)
        {
            return x.Replace("'","").Equals("Y");
        }
    }
}
