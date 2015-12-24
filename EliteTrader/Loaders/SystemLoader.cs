using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EliteTrader.Entities;

namespace EliteTrader.Loaders
{
    public class SystemsLoader
    {
        private readonly string _filepath;

        public SystemsLoader(string filepath)
        {
            _filepath = filepath;
        }

        public Dictionary<string, Entities.System> Load()
        {
            Dictionary <string, Entities.System> systems = new Dictionary<string, Entities.System>();
                
            string[] fileContent = File.ReadAllLines(_filepath);

            fileContent.ToList()
                       .Select(x => x.Split(','))
                       .Skip(1)
                       .Select(x => x.Select(y => y.Trim('\'')).ToArray())
                       .ToList()
                       .ForEach(x => systems.Add(x[0], new Entities.System(x[0], 
                                        new Point3(Convert.ToDouble(x[1], CultureInfo.InvariantCulture), Convert.ToDouble(x[2],CultureInfo.InvariantCulture), Convert.ToDouble(x[3], CultureInfo.InvariantCulture)),
                                        x[4], parseDate(x[5]))));

            return systems;
        }

        private DateTime parseDate(string s)
        {
            DateTime dateTime = DateTime.Now;

            return dateTime;
        }
    }
}
