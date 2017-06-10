using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Models;
using MSDev.Task.Helpers;
using MSDev.Task.Tools;
using Newtonsoft.Json;
using static System.String;

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
		public void StartAsync()
		{

		}


		/// <summary>
		/// 抓取单面视频内容
		/// </summary>
		public async Task<bool> SavePageVideosAsync()
		{
			int totalNumber = Context.C9Articles.Count();
			for (int i = 27800; i <= totalNumber; i = i + 100)
			{
				await SaveVideosAsync(i);
			}
			return true;
		}

		/// <summary>
		/// 根据url抓取video内容
		/// </summary>
		public void SaveVideoByUrl()
		{
			var file = new FileInfo("c9videoErrorout.txt");
			StreamReader stream = file.OpenText();

			string url = stream.ReadLine();
			int i = 1;
			while (!stream.EndOfStream)
			{
				if (!IsNullOrEmpty(url))
				{
					url = url.Trim();
					C9Video re = _helper.GetPageVideoByUrl(url);

					re.Id = Guid.NewGuid();
					Context.C9Videos.Add(re);
					try
					{
						Context.SaveChanges();
						Console.WriteLine($"strat:{i}");
						i++;
					}
					catch (Exception e)
					{
						Log.Write("C9Video.error.txt", url);
					}
				}

				url = stream.ReadLine();
			}
			Console.WriteLine("Done");
		}

		/// <summary>
		/// 根据C9Article 抓取视频数据
		/// </summary>
		/// <param name="skip">领衔量</param>
		/// <param name="number">数量</param>
		/// <returns></returns>
		public async Task<List<C9Video>> SaveVideosAsync(int skip = 0, int number = 100)
		{
			Console.WriteLine($"start:{skip}");
			var C9Articles = Context.C9Articles
				.OrderByDescending(m => m.UpdatedTime)
				.Skip(skip).Take(number).ToList();

			var videoList = new List<C9Video>();
			var lastVideo = Context.C9Videos.OrderByDescending(m => m.UpdatedTime).Take(60).ToList();
			Parallel.ForEach(C9Articles, a =>
			{
				// 过滤非视频数据
				if (a.Duration == null)
				{
					Console.WriteLine("Not Video" + a.Title);
					return;
				}
				// 数据库去重
				if (lastVideo.Any(m => m.SourceUrl == a.SourceUrl))
				{
					return;
				}

				C9Video re = _helper.GetPageVideo(a).Result;
				re.Id = Guid.NewGuid();
				videoList.Add(re);
			});
			try
			{
				Context.AddRange(videoList);
				await Context.SaveChangesAsync();
				return videoList;
			}
			catch (Exception e)
			{
				Log.Write("C9VideoSaveError.txt", e.Message);
				Console.WriteLine(e);
			}
			return videoList;
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
				return reList;
			}
		}

	}
}
