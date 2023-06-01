using AuthenticationExample.CookieAuthenticatioin.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using System.Text.Json;

namespace AuthenticationExample.CookieAuthenticatioin.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        /// <summary>
        /// 初始化Home控制器
        /// </summary>
        /// <param name="logger">日之前</param>
        /// <param name="diagnosticContext">During request processing, additional properties can be attached to the completion event using IDiagnosticContext.Set()</param>
        public HomeController(ILogger<HomeController> logger, IDiagnosticContext diagnosticContext) {
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public IActionResult Index() {
            _diagnosticContext.Set("Action", "HomeController.Index");
            return View();
        }

        [Authorize]
        public IActionResult Privacy() {
            _logger.LogInformation("正在处理请求[{Controller}-{Action}]", "HomeController", "Privacy");
            ViewData["Claims"] = JsonSerializer.Serialize(HttpContext.User.Claims.Select(c => new { c.Type, c.Value }));
            return View();
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <returns>注销结果</returns>
        public async Task<IActionResult> Logout() {
            if (HttpContext.User?.Identity?.IsAuthenticated ?? false) {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            _logger.LogError("系统发生了未处理错误:{@ErrorViewModel}", args: model);
            return View(model);
        }
    }
}