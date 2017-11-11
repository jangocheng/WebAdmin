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
            var list =await _helper.GetEventsAsync();
            Log.Write("output.json", JsonConvert.SerializeObject(list));
        }


        public void GetVideos()
        {

        }

    }
}
