using System;
using System.Collections.Generic;
using System.Linq;
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
		/// <summary>
		/// 开始执行任务
		/// </summary>
		public void Start()
		{


		}


		/// <summary>
		/// 抓取单面视频内容
		/// </summary>
		public void SavePageVideos()
		{
			C9Article c9Article = Context.C9Articles.First();
			C9Video re = _helper.GetPageVideo(c9Article);
			Console.WriteLine(re.ToString());

		}


		/// <summary>
		/// C9 AllContent的抓取入库
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public async Task<bool> SaveArticles(int page)
		{
			try
			{
				List<C9Article> articlielList = await _helper.GetArticleListAsync(page);
				if (articlielList.Count < 1)
				{
					return false;
				}
				//TODO:去重操作
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
