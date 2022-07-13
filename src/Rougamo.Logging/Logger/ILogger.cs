using System;
using System.Collections.Generic;
using System.Text;

namespace Rougamo.Logging.Logger
{
    public interface ILogger
    {
        void Log(int level, string logcontext);
    }
}
