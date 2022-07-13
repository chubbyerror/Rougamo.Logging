using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rougamo.Logging;

[assembly: Rougamo.Logging.Attribute.Logging]
namespace TestRogamo
{
    
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        [Rougamo.IgnoreMo]
        static void Main()
        {
            //极简注册日志方法
            "testrougamo".UseRougamoLog<Logger>();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
