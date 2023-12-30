using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_Desktop
{
    internal class ProcessHandler
    {
        static internal void StartProcesses(string programPath)
        {
            ProcessStartInfo psi = new ProcessStartInfo(programPath);
            Process pro = new Process();
            pro.StartInfo = psi;
            try
            {
                pro.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
