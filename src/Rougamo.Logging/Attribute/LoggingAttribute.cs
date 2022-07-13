using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Rougamo.Logging.Attribute
{
    [Rougamo.IgnoreMo]
    public class LoggingAttribute : MoAttribute
    {
        public override AccessFlags Flags => AccessFlags.All;

        private readonly Guid _id;

        public string LoggerName { get; set; }

        public LoggingAttribute()
        {
            _id = Guid.NewGuid();
        }

        public override void OnEntry(MethodContext context)
        {
            Task.Factory.StartNew(async () => {
                await Log(context, Logtype.Entry);
            });
            base.OnEntry(context);
        }

        public override void OnException(MethodContext context)
        {
            Task.Factory.StartNew(async () => {
                await Log(context, Logtype.Exception);
            });
            base.OnException(context);
        }

        public override void OnExit(MethodContext context)
        {
            Task.Factory.StartNew(async () => {
                await Log(context, Logtype.Exit);
            });
            base.OnExit(context);
        }

        public override void OnSuccess(MethodContext context)
        {
            Task.Factory.StartNew(async () => {
                await Log(context, Logtype.Success);
            });
            base.OnSuccess(context);
        }

        private async Task Log(MethodContext context, Logtype logtype)
        {
            var logTime = DateTime.Now;
            await Task.Run(() =>{

                StringBuilder msg = new StringBuilder();
                List<string> msgs = new List<string>();

                msgs.Add($"\"Time\":\"{logTime:yyyy-MM-dd HH:mm:ss fff}\"");
                msgs.Add($"\"Id\":\"{_id}\"");
                msgs.Add($"\"LoggerName\":\"{LoggerExpansion.LoggerName}\"");
                msgs.Add($"\"Logtype\":{logtype}");

                List<string> fields = new List<string>();
                if (LoggerExpansion.CustomFields  != null)
                {
                    foreach (var item in LoggerExpansion.CustomFields())
                    {
                        fields.Add($"{{\"{item.Key}\":\"{item.Value}\"}}");
                    }
                    if (fields.Count>0)
                    {
                        msgs.Add($"\"CustomFields\":[{string.Join(",", fields)}]");
                    }
                }

                msgs.Add( LoggerExpansion.Formatter.FromatContextWithArguments(context));


                if (logtype== Logtype.Success && context.ReturnValue!=null)
                {
                    msgs.Add($"\"Returns\":\"{Newtonsoft.Json.JsonConvert.SerializeObject(context.ReturnValue)}\"");
                }

                msg.Append("{").Append(string.Join(",",msgs)).Append("}");

                if (logtype == Logtype.Exception)
                {
                    LoggerExpansion.Logger.Log((int)Logger.LogLevel.Exception, msg.ToString());
                }
                else
                {
                    LoggerExpansion.Logger.Log((int)Logger.LogLevel.Debug, msg.ToString());
                }
            });
        }
    }
}
