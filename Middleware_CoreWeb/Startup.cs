using JwtAuthenticationHelper.Extensions;
using JwtAuthenticationHelper.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CoreWebApi",
                    Version = "v1",
                    Description = "ASP.NET Core WebApi"
                });

                // ��ȡxml��Ϣ ʹ�÷����ȡxml�ļ�����������ļ���·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                c.IncludeXmlComments(xmlPath, true);

                // ����swagger��֤����
                //var security = new Dictionary<string, IEnumerable<string>> { { "CoreAPI", new string[] { } }, };
                //c.AddSecurityRequirement(security);
                //c.AddSecurityDefinition("CoreAPI", new ApiKeyScheme
                //{
                //    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ���·�����Bearer {token} ���ɣ�ע������֮���пո�",
                //    Name = "Authorization",//jwtĬ�ϵĲ�������
                //    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                //    Type = "apiKey"
                //});
            });

            var tokenOptions = new TokenOptions(
               Configuration["Token:Audience"],
               Configuration["Token:Issuer"],
               Configuration["Token:SigningKey"]);

            services.AddJwtAuthenticationWithProtectedCookie(
                tokenOptions);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequiresAdmin",
                    policy =>
                    policy.RequireClaim("HasAdminRights"));
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
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            //// �����м����������Swagger
            //// �����м����������SwaggerUI��ָ��Swagger JSON�ս��
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
                c.RoutePrefix = string.Empty;//���ø��ڵ����
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}");
            });
        }
    }
}