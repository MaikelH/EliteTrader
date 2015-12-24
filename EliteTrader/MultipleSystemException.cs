using System;

namespace EliteTrader
{
    public class MultipleSystemException : Exception
    {
        public MultipleSystemException(string s) : base(s)
        {
            
        }
    }
}