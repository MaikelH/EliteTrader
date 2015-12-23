using System;

namespace EliteTrader.Entities
{
    public class Station
    {
        public Station(string systemname, string stationName, int distanceFromStar, bool blackMarket, string maxPadSize, bool market, bool shipyard, DateTime modified, bool outfitting, bool rearm, bool refuel, bool repair, bool planetary)
        {
            Systemname = systemname;
            StationName = stationName;
            DistanceFromStar = distanceFromStar;
            BlackMarket = blackMarket;
            MaxPadSize = maxPadSize;
            Market = market;
            Shipyard = shipyard;
            Modified = modified;
            Outfitting = outfitting;
            Rearm = rearm;
            Refuel = refuel;
            Repair = repair;
            Planetary = planetary;
        }

        public string Systemname { get; }

        public string StationName { get; }

        public int DistanceFromStar { get; }

        public bool BlackMarket { get; }

        public string MaxPadSize { get; }

        public bool Market { get; }

        public bool Shipyard { get; }

        public DateTime Modified { get; }

        public bool Outfitting { get; }

        public Boolean Rearm { get; }

        public bool Refuel { get; }

        public bool Repair { get; }

        public bool Planetary { get; }
    }
}