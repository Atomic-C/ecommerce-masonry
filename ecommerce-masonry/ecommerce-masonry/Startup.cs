using Masonry_Data_Access;
using Masonry_Data_Access.Repository;
using Masonry_Data_Access.Repository.IRepository;
using Masonry_Utility;
using Masonry_Utility.BrainTree;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ecommerce_masonry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // This is the configuration we use.

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders().AddDefaultUI() // AddDefaultTokenProviders provides the tokens when we forget the password
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddHttpContextAccessor(); 

            services.AddSession(Options =>
            {
                // Here we configure out options for the session
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;  
            });
            services.Configure<BrainTreeSettings>(Configuration.GetSection("BrainTree")); // We add the json settings in BrainTreeSettings class

            // TODO: Try using GetSection for mailjet when you have time, so we register everything in our container here :D

            services.AddSingleton<IBrainTreeGate, BrainTreeGate>(); // We register interface for gateway!

            services.AddScoped<ICategoryRepository, CategoryRepository>(); // We have to add this so we do not get InvalidOperationException: Unable to resolve service for type
              // Scoped Lifetime is also useful when ever we deal with the database, when registering, because scope stays for one request
                                                                           // Transient would create a new object all the time.
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
            services.AddScoped<IInquiryDetailsRepository, InquiryDetailsRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddControllersWithViews(); 
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // Below we configure out session
            // By default only string and integer can be stored on .NETCORE. So we can add extension methods on session to configure this.
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
