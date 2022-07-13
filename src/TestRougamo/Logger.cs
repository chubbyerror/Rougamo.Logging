using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRogamo
{
    [Rougamo.IgnoreMo]
    public class Logger : Rougamo.Logging.Logger.ILogger
    {
        public void Log(int level, string logcontext)
        {
            Console.WriteLine(logcontext);
        }
    }
}
