using System;

namespace Config4Net
{
    public class InvalidConfigTypeException : Exception
    {
        public InvalidConfigTypeException()
        {
        }

        public InvalidConfigTypeException(string msg) : base(msg)
        {
        }
    }
}