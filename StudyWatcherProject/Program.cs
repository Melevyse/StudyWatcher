using System.Net.Mime;
using StudyWatcherProject.EFC;
using StudyWatcherProject.Models;
using System;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//app.Run();

namespace StudyWatcherProject
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }    
}





/*
using (SqlReportingContext db = new SqlReportingContext())
{
    User arkadey = new User { Fio = "Arkadey", GroupStudent = "IKMO0421", UserLogin = "Arakdey", UserPassword = "123456"};
    User alice = new User { Fio = "Alice", GroupStudent = "IKMO0421", UserLogin = "Alice", UserPassword = "123456"};
    db.Add(arkadey);
    db.Add(alice);
    db.SaveChanges();
    var users = db.User.ToList();
    Console.WriteLine("Список объектов:");
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.GroupStudent}");
    }
}
*/


