using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_Desktop
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //MainWindow mainWindow = new MainWindow();
            try
            {
                Application.Run(new MainWindow());
            }
            catch (Exception e)
            {
                MessageBox.Show("The Application run error,please check the associated environment." + "\n" + e.Message);
            }
        }
    }
}
