using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Models;
using MSDev.Task.Helpers;
using MSDev.Task.Tools;
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
		public async Task<bool> SavePageVideosAsync()
		{
			var totalNumber = Context.C9Articles.Count();
			for (int i = 693; i < totalNumber; i = i + 10)
			{
				await SaveOneVideoAsync(i);
			}
			return true;
		}

		/// <summary>
		/// 插入一条视频数据
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public async Task<bool> SaveOneVideoAsync(int i)
		{
			Console.WriteLine($"start:{i}");
			var C9Articles = Context.C9Articles
				.OrderByDescending(m => m.UpdatedTime)
				.Skip(i).Take(10).ToList();


			foreach (C9Article C9Article in C9Articles)
			{
				//过滤非视频数据
				if (C9Article.Duration == null)
				{
					Console.WriteLine("Not Video" + C9Article.Title);
					return false;
				}

				//if (Context.C9Videos.Any(m => m.Title == re.Title))
				//{
				//	Console.WriteLine($"Exist:{re.Title}");
				//	return false;
				//}
				C9Video re = _helper.GetPageVideo(C9Article);

				re.Id = Guid.NewGuid();
				Context.C9Videos.Add(re);
				Log.Write("c9videoSuccess.txt", re.SourceUrl);

			}

			try
			{
				await Context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return true;
		}


		/// <summary>
		/// C9 AllContent的抓取入库
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public async Task<List<C9Article>> SaveArticles(int page)
		{
			var reList = new List<C9Article>();
			try
			{
				List<C9Article> articlielList = await _helper.GetArticleListAsync(page);
				reList = articlielList.ToList();

				//TODO:去重操作
				var lastAritles = Context.C9Articles
					.OrderByDescending(m => m.UpdatedTime)
					.Take(12 * 5)
					.ToList();

				foreach (C9Article article in articlielList)
				{
					foreach (C9Article lastAritle in lastAritles)
					{
						if (article.SourceUrl == lastAritle.SourceUrl)
						{
							reList.Remove(article);
							Console.WriteLine($"repeat:{article.Title}");
							break;
						}
					}
				}

				if (reList.Count > 0)
				{
					Context.C9Articles.AddRange(reList);
					int re = Context.SaveChanges();
					Console.WriteLine(re <= 0 ? "save failed" : $"task:{page} finish!");
				}

				return reList;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine(JsonConvert.SerializeObject(reList));
				Console.WriteLine("当前:" + page);
				throw;
				return null;
			}
		}

	}
}
