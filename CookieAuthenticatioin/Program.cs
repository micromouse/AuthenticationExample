using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Serilog.Events;

namespace AuthenticationExample.CookieAuthenticatioin {
    /// <summary>
    /// Cookie»œ÷§
    /// </summary>
    public class Program {
        /// <summary>
        /// main
        /// </summary>
        /// <param name="args">≤Œ ˝</param>
        public static void Main(string[] args) {
            //≈‰÷√Serilog
            ConfigureSerilog();

            try {
                Log.Information("Starting web application");

                var builder = WebApplication.CreateBuilder(args);

                //≈‰÷√Serilog
                builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console());

                // Add services to the container.
                builder.Services
                    .AddAuthentication(options =>
                    {
                        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    }).AddCookie(options =>
                    {
                        options.Cookie.Name = "CookieAuthenticatioinSample";
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.LoginPath = "/Account/Login";
                        options.AccessDeniedPath = "/Account/Denied";
                    });
                builder.Services
                    .AddAuthorization()
                    .AddTransient<IHttpContextAccessor, HttpContextAccessor>();
                // Add services to the container.                
                builder.Services.AddControllersWithViews();

                var app = builder.Build();

                app.UseSerilogRequestLogging(options =>
                {
                    // Customize the message template
                    options.MessageTemplate = "[{RemoteIpAddress}-{LoginUser}-{RequestMethod}-{StatusCode}-{Elapsed}]Handled {RequestScheme}//{RequestHost}{RequestPath}";

                    // Emit debug-level events instead of the defaults
                    //options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;

                    // Attach additional properties to the request completion event
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                        diagnosticContext.Set("LoginUser", httpContext.User.Identity?.Name ?? "Œ¥µ«¬º");
                    };
                });

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment()) {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
            } catch (Exception ex) {
                Log.Fatal(ex, "Application terminated unexpectedly");
            } finally {
                Log.CloseAndFlush();
            }

        }

        /// <summary>
        /// ≈‰÷√Serilog
        /// </summary>
        static void ConfigureSerilog() {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
        }
    }
}