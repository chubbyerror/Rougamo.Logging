# Rougamo.Logging - 肉夹馍.特性标记日志扩展

### rougamo是什么
静态代码织入AOP，.NET最常用的AOP应该是Castle DynamicProxy，rougamo的功能与其类似，但是实现却截然不同，
DynamicProxy是运行时生成一个代理类，通过方法重写的方式执行织入代码，rougamo则是代码编译时直接修改IL代码，
.NET静态AOP方面有一个很好的组件PostSharp，rougamo的注入方式也是与其类似的。

详见https://github.com/inversionhourglass/Rougamo

### Rougamo.Logging是什么
Rougamo.Logging 是基于肉夹馍静态织入编写的快速特性标记日志扩展工具，使用本扩展可以快速的为你的程序绑定和解除日志。


标记和注册扩展
```csharp
//使用特性标记日志
[assembly: Rougamo.Logging.Attribute.Logging]

//极简注册日志方法
"testrougamo".UseRougamoLog<Logger>();
```
实现日志接口
```csharp
//您仅需要实现日志接口
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yournamespace
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
```
允许实现自己的IFormatter以定制自己的日志需求
```csharp
//允许您实现自己的IFormatter以定制自己的日志需求
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yournamespace
{
    [IgnoreMo]
    internal class Formatter : Rougamo.Logging.Formatter.IFormatter
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
```

更多细节请探索 https://github.com/inversionhourglass/Rougamo 肉夹馍 
