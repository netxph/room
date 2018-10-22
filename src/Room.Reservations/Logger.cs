using System.Diagnostics;

namespace Room.Reservations
{
    public class Logger
    {
        public static void Log(string message)
        {
            Trace.WriteLine(message);
        }
    }
}