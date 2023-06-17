using System.Net.Mime;
using StudyWatcherProject.EFC;
using StudyWatcherProject.Models;
using System;
using Microsoft.AspNetCore;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//app.Run();

namespace StudyWatcherProject
{
    public static class Program
    {
        /*
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            //WebHost.CreateDefaultBuilder().UseUrls("http://*:5000;http://localhost:5001");
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        */
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSetting("http_port", "5000")
                .UseUrls("http://*:5000;http://localhost:5000")
                .UseStartup<Startup>();
        
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


