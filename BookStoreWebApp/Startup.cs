using BookStoreWebApp.Data;
using BookStoreWebApp.Helper;
using BookStoreWebApp.Models;
using BookStoreWebApp.Repository;
using BookStoreWebApp.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")
                ));
            services.AddControllersWithViews();

            //For Identity Core Connecting with the database
            services.AddIdentity<AccountUser, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreContext>().AddDefaultTokenProviders();

            //For Custom Validations on the Identity Core Password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;
            });
            //Configure the Application Cookie
            services.ConfigureApplicationCookie(Config =>
            {
                //     The LoginPath property is used by the handler for the redirection target when
                //     handling ChallengeAsync. The current url which is added to the LoginPath as a
                //     query string parameter named by the ReturnUrlParameter. Once a request to the
                //     LoginPath grants a new SignIn identity, the ReturnUrlParameter value is used
                //     to redirect the browser back to the original url.
                Config.LoginPath = Configuration["Application:LoginPath"];
            });
#if DEBUG
            //This will Gets Works Only in Development mode not in Production or Staging mode
            services.AddRazorPages().AddRazorRuntimeCompilation().AddViewOptions(options=>
            {
                //This Configuration is used to Enable and Disable the Client Side Validation
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });
#endif
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserClaimsPrincipalFactory<AccountUser>, ApplicationUserClaimsPrincipleFactory>();   //Helper For the Claims for storing users Info
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //If there is an Another static file folder in the folder structure other than named wwwroot than this configuration has to be done
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Staticfile")),
            //    RequestPath = "/Staticfile"
            //});
            

            app.UseRouting();
            //UseAuthentication will add the authentication in the middleware of the request pipeline
            app.UseAuthentication();
            //Authorization is used on Action method that if user will be signed in then and only then they can access
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern:"all-Books",
                //    defaults: new { controller="Book",action="Index"});
            });
        }
    }
}
