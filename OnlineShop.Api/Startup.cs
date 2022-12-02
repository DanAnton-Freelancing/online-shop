using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Api.Extensions;
using OnlineShop.Api.Helpers;
using OnlineShop.Primary.Adapters;
using OnlineShop.Secondary.Adapters;
using OnlineShop.Secondary.Adapters.Extensions;

namespace OnlineShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                                       {
                                           x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
                    .AddJwtBearer(x =>
                                  {
                                      x.RequireHttpsMetadata = false;
                                      x.SaveToken = true;
                                      x.TokenValidationParameters = new TokenValidationParameters
                                                                    {
                                                                        ValidateIssuerSigningKey = true,
                                                                        IssuerSigningKey =
                                                                            new SymmetricSecurityKey(key),
                                                                        ValidateIssuer = false,
                                                                        ValidateAudience = false
                                                                    };
                                  });
            services.AddSecondaryAdapters(appSettings.ConnectionString);
            services.AddPrimaryAdapters(appSettings.Secret, Configuration);
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseCustomSwaggerUi();
                app.UseDeveloperExceptionPage();
            }
            else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(x => x
                             .AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.SeedInitialData();
        }
    }
}