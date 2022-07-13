using System;
using System.Collections.Generic;
using System.Text;

namespace Rougamo.Logging.Formatter
{
    public interface IFormatter
    {
        string FromatContextWithArguments(Context.MethodContext context);
    }
}
