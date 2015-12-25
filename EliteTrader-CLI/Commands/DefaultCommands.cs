using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteTrader;

namespace EliteTrader_CLI.Commands
{
    public static class DefaultCommands
    {
        public static string nav(string from, string to)
        {
            EliteTrader.Entities.System fromSystem = Program.TraderApplication.ResolveSystem(from);
            EliteTrader.Entities.System toSystem = Program.TraderApplication.ResolveSystem(to);
            return "Nav";
        }

        public static string jp(double distance)
        {
            Program.TraderApplication.JumpRange = distance;

            return $"[*] Jump distance set to {distance} ly";
        }

        public static string closest(string startingSystem, double distance = -1)
        {
            try
            {
                EliteTrader.Entities.System startSystem = Program.TraderApplication.ResolveSystem(startingSystem);

                if (Math.Abs(distance - (-1.0)) < 0.00005)
                {
                    distance = Program.TraderApplication.JumpRange;
                }

                Dictionary<EliteTrader.Entities.System, double> systems = Program.TraderApplication.FindClosestSystems(startSystem, distance);

                StringBuilder builder = new StringBuilder();

                builder.AppendLine("Closest systems: ");

                foreach (KeyValuePair<EliteTrader.Entities.System, double> keyValuePair in systems)
                {
                    builder.Append("-> ");
                    builder.AppendFormat("{0:F2}", keyValuePair.Value);
                    builder.Append(" ly");
                    builder.Append(" - ");
                    builder.AppendLine(keyValuePair.Key.Name);
                }

                return builder.ToString();
            }
            catch (MultipleSystemException e)
            {
                return "[!] " + e.Message;
            }
        }
    }
}
