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
}