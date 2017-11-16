using MSDev.DB.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;
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

        public async Task GetEeventsAsync()
        {
            await _helper.GetEventsAsync();
        }


        public void GetVideos()
        {

        }

	}
}
