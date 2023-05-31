using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AuthenticationExample.CookieAuthenticatioin.Controllers {
    /// <summary>
    /// 错误处理控制器
    /// </summary>
    public class ErrorController : Controller {
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// 初始化错误处理控制器
        /// </summary>
        /// <param name="accessor"><see cref="IHttpContextAccessor"/></param>
        public ErrorController(IHttpContextAccessor accessor) {
            _accessor = accessor;
        }

        /// <summary>
        /// Http状态代码处理器
        /// </summary>
        /// <param name="statusCode">Http状态代码</param>
        /// <returns>视图</returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode) {
            var statusCodeReExecuteFeature = _accessor.HttpContext?.Features.GetRequiredFeature<IStatusCodeReExecuteFeature>();

            if (statusCode == (int)HttpStatusCode.NotFound) {
                ViewBag.ErrorMessage = $"抱歉，你访问的页面[{statusCodeReExecuteFeature?.OriginalPath}]不存在。";
            }
            return View("NotFound");
        }
    }
}
