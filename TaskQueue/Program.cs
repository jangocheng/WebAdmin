using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MSDev.DataAgent;
using MSDev.TaskQueue.News;

namespace MSDev.TaskQueue
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //确保数据库建立
            var dbContext = new AppDbContext();
            dbContext.Database.Migrate();
            Run();
            //Console.ReadLine();
            Thread.Sleep(1000*10);
        }


        public static async void Run()
        {
            var task = new BingNewsTask();
            await task.GetNews("微软");
            Console.WriteLine("Done");
        }
    }
}
