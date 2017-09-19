using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using MSDev.DB;
using System.Linq;

namespace WebAdmin.Controllers
{
    public class TaskController : BaseController
    {

        readonly AppDbContext _context;
        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task RunTask()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];
                var result = await webSocket.ReceiveAsync(
                  new ArraySegment<byte>(buffer), CancellationToken.None);
                var runner = new TaskRunner(webSocket);

                while (!result.CloseStatus.HasValue)
                {

                    var msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                    if (msg == null)
                    {
                        continue;
                    }

                    await runner.Run(msg);
                    result = await webSocket.ReceiveAsync(
                      new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        }

        /// <summary>
        /// 迁移存储
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult MigrationOSS()
        {
            var list = _context.Resource.ToList();

            foreach (var item in list)
            {
                var newImgUrl = item.Imgurl.Replace("saledazuredisks145.blob.core.windows.net/source/", "msdevccdiag579.blob.core.windows.net/msdev-image/");
                item.Imgurl = newImgUrl;
            }

            _context.UpdateRange(list);
            var re = _context.SaveChanges();
            return Json(re);

        }
    }

}
