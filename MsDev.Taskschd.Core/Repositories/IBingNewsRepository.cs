using System.Collections.Generic;
using MsDev.Taskschd.Core.Models;
using System.Threading.Tasks;

namespace MsDev.Taskschd.Core.Repositories
{
    public interface IBingNewsRepository : IRepository<BingNews>
    {
        //获取最近新闻标题
        Task<List<string>> GetRecentTitlesAsync(int days);

        int DelRange(string[] ids);
    }
}