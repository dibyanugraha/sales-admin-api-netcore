namespace SalesAdmin
{
    using System.Text;
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
    using SalesAdmin.Models;

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

            services.AddTransient<SalesHeaderRepository>(
                _ => new SalesHeaderRepository(
                    Configuration.GetConnectionString("SalesAdmin")));

            var mapper = new MapperConfiguration(b =>
            {
                b.CreateMap<SalesHeader, SalesHeaderResponse>();

            }).CreateMapper();

            services.AddSingleton(mapper);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
