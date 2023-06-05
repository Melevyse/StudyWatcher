namespace StudyWatcherFormsAdmin;

public class InfoWorkStation
{
    public string NameLocation { get; set; }
    public string Info { get; set; }

    public InfoWorkStation(string nameLocation, string infoList, ListView listView)
    {
        this.NameLocation = nameLocation;
        this.Info = infoList;
        var message = new ListViewItem(nameLocation);
        message.SubItems.Add(infoList);
        listView.Items.Add(message);
    }
}