using CZGL.Auth.Interface;
using CZGL.Auth.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Middleware_CoreWeb.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace Middleware_CoreWeb
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
            services.AddControllersWithViews();
            //����Swagger
            //ע��Swagger������������һ��Swagger �ĵ�
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "CoreWebApi",
            //        Version = "v1",
            //        Description = "ASP.NET Core WebApi"
            //    });

            //    // ��ȡxml��Ϣ ʹ�÷����ȡxml�ļ�����������ļ���·��
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    // ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
            //    c.IncludeXmlComments(xmlPath, false);
            //    ////����swagger��֤���� ApiKeyScheme
            //    //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    //{
            //    //    Description = "Ȩ����֤(���ݽ�������ͷ�н��д���) �����ṹ: \"Authorization: Bearer {token}\"",
            //    //    Name = "Authorization",//jwtĬ�ϵĲ�������
            //    //    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
            //    //    Type = "apiKey"
            //    //});//Authorization������
            //});

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IJWTAuthentication, JWTAuthentication>();
            services.AddScoped<IJWTTokenService, JWTTokenService>();
            services.AddScoped<IJWTIdentityService, JWTIdentityService>();

            var jwtSetting = new Middleware_Tool.JwtSetting();
            Configuration.Bind("JwtSetting", jwtSetting);

            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = jwtSetting.Issuer,
                       ValidAudience = jwtSetting.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey)),
                       ClockSkew = TimeSpan.Zero
                   };
               });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"))
            });

            // �����м����������Swagger
            app.UseSwagger();
            // �����м����������SwaggerUI��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;//���ø��ڵ����
            });

            // ��֤��Ȩ
            app.UseAuthorization();
            app.UseAuthentication();

            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseMiddleware<RoleMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
                      {
                          endpoints.MapControllerRoute(
                              name: "default",
                              pattern: "{controller=Home}/{action=Login}/{id?}");
                      });
        }
    }
}