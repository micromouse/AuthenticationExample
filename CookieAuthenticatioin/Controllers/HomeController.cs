using AuthenticationExample.CookieAuthenticatioin.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

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

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            _logger.LogError("系统发生了未处理错误:{@ErrorViewModel}", args: model);
            return View(model);
        }
    }
}