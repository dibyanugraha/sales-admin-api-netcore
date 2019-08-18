namespace SalesAdmin
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using SalesAdmin.Authentication;
    using SalesAdmin.Data;
    using SalesAdmin.Data.Dapper;
    using SalesAdmin.Models.SalesHeader;
    using System.Collections.Generic;
    using System.Text;

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
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(b =>
            {
                var key = Configuration.GetValue<string>("Authentication:Jwtkey");
                b.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddDbContext<UserContext>(a =>
            {
                a.UseMySQL(Configuration.GetConnectionString("SalesAdmin"));
            });
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(a =>
            {
                a.Password.RequireDigit = false;
                a.Password.RequiredLength = 6;
                a.Password.RequireLowercase = false;
                a.Password.RequireNonAlphanumeric = false;
                a.Password.RequireUppercase = false;
            });
            services.AddTransient<ISalesHeaderRepository>(
                _ => new SalesHeaderRepository(
                    Configuration.GetConnectionString("SalesAdmin")));
            services.AddTransient<ISalesLineRepository>(
                _ => new SalesLineRepository(
                    Configuration.GetConnectionString("SalesAdmin")));

            var mapper = new MapperConfiguration(b =>
            {
                b.CreateMap<SalesHeader, SalesHeaderResponse>();
                //b.CreateMap<IEnumerable<SalesHeader>, SalesHeaderListResponse>();
                b.CreateMap<SalesLine, SalesLineRepository>();
            }).CreateMapper();

            services.AddSingleton(mapper);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddDbContext<SalesAdminContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("SalesAdminContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
