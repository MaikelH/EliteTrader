using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
