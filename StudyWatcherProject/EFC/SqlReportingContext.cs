using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.EFC;

public class SqlReportingContext : DbContext
{
    
    public DbSet<UserStudent> UserStudent { get; set; }
    public DbSet<WorkStation> WorkStation { get; set; }
    public DbSet<ProcessWS> ProcessWS { get; set; }
    public DbSet<ProcessBan> ProcessBan { get; set; }
    
    public SqlReportingContext(DbContextOptions<SqlReportingContext> options)
        : base(options)
    {
    }

    public SqlReportingContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=studywatcherdb;Username=postgres;Password=kr4k4z4br4");
    }
}