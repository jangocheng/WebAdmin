using MSDev.DB.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.String;
namespace MSDev.Work.Tasks
{
    public class C9EvnentTask : MSDTask
    {
        private readonly C9Helper _helper;
        public C9EvnentTask()
        {
            _helper = new C9Helper();
        }

        public void DailyUpdate()
        {

        }
        /// <summary>
        /// C9 event 目录信息
        /// </summary>
        /// <returns></returns>
        public async Task GetEeventsAsync()
        {
            var list = await _helper.GetEventsAsync();
            try
            {
                foreach (var item in list)
                {
                    if (Context.C9Event.Any(m => m.TopicName.Equals(item.TopicName))) continue;

                    Context.C9Event.Add(item);
                }
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.Source);
            }
        }

        /// <summary>
        /// 获取EventVideos
        /// </summary>
        public async Task GetVideosAsync()
        {
            var events = Context.C9Event.ToList();
            foreach (var item in events)
            {
                Console.WriteLine("start get:" + item.TopicName);
                var eventVideos = await _helper.GetEventVideosAsync(item);
                if (eventVideos.Count > 0)
                {
                    Log.Write("output.json", JsonConvert.SerializeObject(eventVideos));
                    Context.EventVideo.AddRange(eventVideos);
                    try
                    {
                        Context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source + e.Message + e.StackTrace);
                    }
                }

            }

        }
    }
}
