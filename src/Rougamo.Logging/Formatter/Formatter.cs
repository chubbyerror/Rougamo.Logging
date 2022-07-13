using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rougamo.Logging.Formatter
{
    [IgnoreMo]
    internal class Formatter : IFormatter
    {
        public string FromatContextWithArguments(Context.MethodContext context)
        {
            List<string> args = new List<string>();
            foreach (var item in context.Arguments)
            {
                //反射获取属性
                System.Reflection.PropertyInfo[] properties = item.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                //反射获取方法
                System.Reflection.MethodInfo[] methods = item.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                //判断InvokeRequired
                if ((bool)(properties?.FirstOrDefault(c => c.Name == "InvokeRequired")?.GetValue(item) ?? false))
                {
                    //反射委托至 委托方法，解决跨线程问题
                    methods.FirstOrDefault(c => c.Name == "Invoke" && !c.IsVirtual).Invoke(item, System.Reflection.BindingFlags.Default, null, new object[]{ new Action(() =>
                    {
                        args.Add(JsonConvert.SerializeObject(item, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    }) }, null);
                }
                else
                {
                    args.Add(JsonConvert.SerializeObject(item, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                }
            }
            return $"\"Method\":\"{context.Method.DeclaringType.FullName}.{context.Method.Name}(" + $"{string.Join(",", args)}" + $")";
        }
    }
}
