using System;

namespace EliteTrader
{
    public class EliteTraderEventArgs : EventArgs
    {
        public string Message { get; set; }

        public EliteTraderEventArgs(string message)
        {
            Message = message;
        }
    }
}