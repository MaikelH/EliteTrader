using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteTrader.Entities
{
    public class System
    {
        public System(string name, Point3 location, string addedBy, DateTime modifiedDateTime)
        {
            Name = name;
            Location = location;
            AddedBy = addedBy;
            ModifiedDateTime = modifiedDateTime;
        }

        public Point3 Location { get; }

        public string Name { get; }

        public string AddedBy { get;  }

        public DateTime ModifiedDateTime { get; }
    }
}
