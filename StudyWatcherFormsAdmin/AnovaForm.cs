using Accord.Statistics.Testing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StudyWatcherFormsAdmin
{
    public partial class AnovaForm : Form
    {
        public AnovaForm(string[] nameProcessList, double[] countProcessList, AnovaSourceCollection anovaTable)
        {
            dataGridView1.DataSource = anovaTable;
            for (int iter = 0; iter < nameProcessList.Length; iter++)
            {
                ListViewItem item = new ListViewItem(nameProcessList[iter]);
                item.SubItems.Add(countProcessList[iter].ToString());
                listView1.Items.Add(item);
            }
            InitializeComponent();
        }
    }
}
