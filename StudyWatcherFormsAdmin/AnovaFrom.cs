using System.Windows.Forms;
using Accord.Statistics.Testing;

namespace StudyWatcherFormsAdmin;

public partial class AnovaFrom : Form
{
    public AnovaFrom(string[] nameProcessList, double[] countProcessList, AnovaSourceCollection anovaTable)
    {
        InitializeComponent();
        dataGridView1.DataSource = anovaTable;
        for (int iter = 0; iter < nameProcessList.Length; iter++) 
        { 
            ListViewItem item = new ListViewItem(nameProcessList[iter]);
            item.SubItems.Add(countProcessList[iter].ToString());
            listView1.Items.Add(item);
        }
    }
}