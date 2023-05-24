using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyWatcherFormsUser
{
    public partial class Banner : Form
    {
        public Banner()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            labelErrorP1.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - labelErrorP1.Size.Width / 2,
            (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2) - 100);
            labelErrorP2.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - labelErrorP2.Size.Width / 2,
            (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2) - 50);
            TopMostTimer.Start();
        }

        private void TopMostTimer_Tick(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
    }
}
