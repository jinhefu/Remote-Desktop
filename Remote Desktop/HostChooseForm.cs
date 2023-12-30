using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_Desktop
{
    public partial class HostChooseForm : Form
    {
        public string _pcName;
        public HostChooseForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            //this.button1.BackColor= System.Drawing.Color.FromArgb(211, 219, 244);
            //this.button2.BackColor = System.Drawing.Color.FromArgb(211, 219, 244);
        }
        public void btn_click(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;//将触发此事件的对象转换为该Button对象
            _pcName = button.Text;
            DialogResult= DialogResult.OK;
        }
    }
}
