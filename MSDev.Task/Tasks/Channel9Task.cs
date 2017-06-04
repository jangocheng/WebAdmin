using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSDev.DB.Models;
using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using Newtonsoft.Json;

namespace MSDev.Task.Tasks
{
	public class Channel9Task : MSDTask
	{
		private readonly C9Helper _helper;
		public Channel9Task()
		{
			_helper = new C9Helper();
		}

		public async void Start()
		{

			int pageNumber = await _helper.GetTotalPage();
			Run(2866, pageNumber);

		}


		public async void Run(int currentPage, int total)
		{
			for (int i = currentPage; i <= total; i++)
			{
				bool re = await SaveArticles(i);
				if (re) continue;

				return;
			}
		}


		public async Task<bool> SaveArticles(int page)
		{
			try
			{
				List<C9Article> articlielList = await _helper.GetArticleListAsync(page);
				if (articlielList.Count < 1)
				{
					return false;
				}

				Context.C9Articles.AddRange(articlielList);
				int re = Context.SaveChanges();
				Console.WriteLine(re <= 0 ? "save failed" : $"task:{page} finish!");
				if (re > 0) return true;
				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}

	}
}
