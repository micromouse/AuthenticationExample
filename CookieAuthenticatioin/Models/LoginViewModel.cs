namespace AuthenticationExample.CookieAuthenticatioin.Models {
    /// <summary>
    /// 登录视图模型
    /// </summary>
    public class LoginViewModel {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; init; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; init; } = null!;
        /// <summary>
        /// 登录成功后重定向Url
        /// </summary>
        public string? ReturnUrl { get; init; }
    }
}
