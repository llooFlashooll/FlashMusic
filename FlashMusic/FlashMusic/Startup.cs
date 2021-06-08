using FlashMusic.Database;
using FlashMusic.Models;
using FlashMusic.Services;
using FlashMusic.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashMusic
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // �ڴ˴���ӷ���
        public void ConfigureServices(IServiceCollection services)
        {
            // JWT��ʹ��
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)      // Ĭ����Ȩ��������
                .AddJwtBearer(options =>
                {
                    // options.RequireHttpsMetadata = false;
                    // options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,          // �Ƿ��������ڼ���֤ǩ����
                        ValidateAudience = true,        // �Ƿ���֤ǩ����
                        ValidateLifetime = true,        // �Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true,    // �Ƿ���֤ǩ��
                        ValidIssuer = Configuration["Jwt:Issuer"],      // ǩ���ߣ�ǩ��Token����
                        ValidAudience = Configuration["Jwt:Audience"],  // ������
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),    // �õ����ܺ��key
                        ClockSkew = TimeSpan.Zero
                    };
                });


            // ��Controller�ĺ��ķ���ע�ᵽ������ȥ
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<AppDbContext>(options => {
                options.UseMySQL(Configuration["DbContext:ConnectionString"]);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // ע��AddTransient��AddSingleton��AddScoped������ע�����
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IShoppingRepository, ShoppingRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            // app.UseHttpsRedirection();

            // ������
            app.UseRouting();

            // ����˭
            app.UseAuthentication();

            // ����ʲôȨ��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
