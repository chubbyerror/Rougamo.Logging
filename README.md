# Rougamo.Logging - 肉夹馍.特性标记日志扩展

### rougamo是什么
静态代码织入AOP，.NET最常用的AOP应该是Castle DynamicProxy，rougamo的功能与其类似，但是实现却截然不同，
DynamicProxy是运行时生成一个代理类，通过方法重写的方式执行织入代码，rougamo则是代码编译时直接修改IL代码，
.NET静态AOP方面有一个很好的组件PostSharp，rougamo的注入方式也是与其类似的。
详见https://github.com/inversionhourglass/Rougamo

### Rougamo.Logging是什么
Rougamo.Logging 是基于肉夹馍静态织入编写的快速特性标记日志扩展工具，使用本方法可以快速的为你的程序绑定和解除日志。
