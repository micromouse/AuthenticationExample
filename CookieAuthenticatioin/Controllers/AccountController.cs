using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthenticationExample.CookieAuthenticatioin.Models;

namespace AuthenticationExample.CookieAuthenticatioin.Controllers {
    /// <summary>
    /// Account控制器
    /// </summary>
    public class AccountController : Controller {
        private readonly IHttpContextAccessor _accessor;

        public AccountController(IHttpContextAccessor accessor) {
            _accessor = accessor;
        }

        /// <summary>
        /// Index视图
        /// </summary>
        /// <returns>Index视图</returns>
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// 显示登录视图
        /// </summary>
        /// <returns>登录视图</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = "") {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        /// <summary>
        /// 认证登录
        /// </summary>
        /// <param name="model">登录视图模型</param>
        /// <returns>认证登录结果</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (_accessor.HttpContext == null) {
                throw new HttpRequestException("不存在Http请求上下文，无法进行登录认证");
            }

            if (model.Username == "admin" && model.Password == "admin") {
                var claims = new[] {
                    //new Claim(ClaimTypes.Name, model.Username),
                    new Claim("Username", model.Username),
                    new Claim("ReturnUrl", model.ReturnUrl ?? "/"),
                    new Claim("CreateTime", DateTime.Now.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, nameType: "Username", roleType: "role");
                var principal = new ClaimsPrincipal(claimsIdentity);
                await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Redirect(model.ReturnUrl ?? "/");
            }

            return Redirect($"/Account/Login?returnUrl={model.ReturnUrl}");
        }

        public IActionResult Denied() {
            return View();
        }

        public IActionResult ToDenied() {
            return Forbid();
        }
    }
}
