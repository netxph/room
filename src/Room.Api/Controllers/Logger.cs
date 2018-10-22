using System.Diagnostics;

namespace Room.Api.Controllers
{
    public class Logger
    {
        public static void Log(string message)
        {
            Trace.WriteLine(message);
        }
    }
}