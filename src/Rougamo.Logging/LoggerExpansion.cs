using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rougamo.Logging
{
    [Rougamo.IgnoreMo]
    public static class LoggerExpansion
    {
        public static string LoggerName { set; get; }
        public static Func<Dictionary<string, string>> CustomFields { set; get; }
        public static Formatter.IFormatter Formatter { get; set; }
        public static Logger.ILogger Logger { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="loggername"></param>
        /// <param name="customfields"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string UseRougamoLog<T, T1>(this string loggername, Func<Dictionary<string, string>> customfields = null)
            where T : Logger.ILogger, new()
            where T1 : Formatter.IFormatter, new()
        {
            //强制要求 ILogger 实现类标记 IgnoreMo 特性
            if (typeof(T).GetCustomAttributes(true).All(c => c.ToString().IndexOf("IgnoreMo") ==-1))
            {
                throw new ArgumentException($"Please mark [Rougamo.IgnoreMo] attributes to your ILogger impl.{System.Environment.NewLine}请为您的 ILogger 实现标记 [Rougamo.IgnoreMo] 特性");
            }

            //强制要求 IFormatter 实现类标记 IgnoreMo 特性
            if (typeof(T1).GetCustomAttributes(true).All(c => c.ToString().IndexOf("IgnoreMo") == -1))
            {
                throw new ArgumentException($"Please mark [Rougamo.IgnoreMo] attributes to your IFormatter impl.{System.Environment.NewLine}请为您的 IFormatter 实现标记 [Rougamo.IgnoreMo] 特性");
            }

            Logger = new T();
            Formatter = new T1();

            LoggerName = loggername;
            CustomFields = customfields;

            return LoggerName;
        }
        public static string UseRougamoLog<T>(this string loggername, Func<Dictionary<string, string>> customfields = null)
            where T : Logger.ILogger, new()
        {
            return loggername.UseRougamoLog<T, Formatter.Formatter>(customfields);
        }
    }
}
