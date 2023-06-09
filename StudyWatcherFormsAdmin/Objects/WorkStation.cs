﻿using Accord.Controls;

namespace StudyWatcherFormsAdmin;

public class WorkStation
{
    public string Fio { get; set; }
    public string Group { get; set; }
    public string NameMotherboard { get; set; }
    public string NameCPU { get; set; }
    public string NameRAM { get; set; }
    public string NameHDD { get; set; }
    public string NameVideocard { get; set; }
    public Status Status; 
    public string NameLocation { get; set; }
    public string ConnectionId { get; set; }
    public List<string> ProcessList { get; set; }

    public Image ImageScreen { get; set; }

    public WorkStation(
        string fio,
        string group,
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        Status status,
        string nameLocation,
        string connectionId,
        ListView listView)
    {
        this.NameLocation = nameLocation;
        this.Fio = fio;
        this.Group = group;
        this.NameMotherboard = nameMotherboard;
        this.NameCPU = nameCPU;
        this.NameRAM = nameRAM;
        this.NameHDD = nameHDD;
        this.NameVideocard = nameVideocard;
        this.Status = status;
        this.ConnectionId = connectionId;
        var result = new ListViewItem(this.NameLocation);
        result.SubItems.Add(fio);
        result.SubItems.Add(group);
        result.SubItems.Add(nameMotherboard);
        result.SubItems.Add(nameCPU);
        result.SubItems.Add(nameRAM);
        result.SubItems.Add(nameHDD);
        result.SubItems.Add(nameVideocard);
        result.SubItems.Add(status.ToString());
        result.SubItems.Add(connectionId);
        listView.Items.Add(result);
    }
    
    public void WorkStationUpdate(
        string nameMotherboard,
        string nameCPU,
        string nameRAM,
        string nameHDD,
        string nameVideocard,
        Status status,
        string connectionId,
        ListView listView)
    {
        var workStationViewItem = listView
            .FindItemWithText(this.NameLocation, true, 0);
        this.NameMotherboard = nameMotherboard;
        this.NameCPU = nameCPU;
        this.NameRAM = nameRAM;
        this.NameHDD = nameHDD;
        this.NameVideocard = nameVideocard;
        this.Status = Status.Login;
        this.ConnectionId = connectionId;
        workStationViewItem.SubItems[3].Text = nameMotherboard;
        workStationViewItem.SubItems[4].Text = nameCPU;
        workStationViewItem.SubItems[5].Text = nameRAM;
        workStationViewItem.SubItems[6].Text = nameHDD;
        workStationViewItem.SubItems[7].Text = nameVideocard;
        workStationViewItem.SubItems[8].Text = status.ToString();
        workStationViewItem.SubItems[9].Text = connectionId;
    }

    public void WorkStationUpdate(
        string fio,
        string group,
        Status status,
        ListView listView)
    {
        var workStationViewItem = listView
            .FindItemWithText(this.NameLocation, true, 0);
        this.Fio = fio;
        this.Group = group;
        this.Status = status;
        workStationViewItem.SubItems[1].Text = fio;
        workStationViewItem.SubItems[2].Text = group;
        workStationViewItem.SubItems[8].Text = status.ToString();
    }

    public void WorkStationUpdate(
        Status status,
        ListView listView)
    {
        var workStationViewItem = listView
            .FindItemWithText(this.NameLocation, true, 0);
        this.Status = status;
        workStationViewItem.SubItems[8].Text = status.ToString();
    }
}