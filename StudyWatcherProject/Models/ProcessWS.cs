﻿namespace StudyWatcherProject.Models;

public class ProcessWS
{
    public Guid Id { get; set; }
    public string NameProcess { get; set; }
    public DateTime LastLaunch { get; set; }
    public string NameLocation { get; set; }
}