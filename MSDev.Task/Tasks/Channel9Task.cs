using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using Newtonsoft.Json;

namespace MSDev.Task.Tasks
{
	public class Channel9Task
	{
		private readonly ApiHelper _apiHelper;

		private readonly C9Helper _helper;

		public Channel9Task(ApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
			_helper = new C9Helper();
		}

		public async void Start()
		{

			int pageNumber = await _helper.GetTotalPage();

			for (int i = 0; i < pageNumber; i++)
			{
				 Task(i);
			}
			//Parallel.For(1, pageNumber, Task);
		}


		public async System.Threading.Tasks.Task Task(int page)
		{
			var articlie = await _helper.GetArticleListAsync(page);
			var re = await _apiHelper.Post<int>("/api/manage/channel9", articlie);
			Console.WriteLine("task:" + page);
			Console.WriteLine(re.ErrorCode == 0 ? "success" : re.Msg);
		}

	}
}
