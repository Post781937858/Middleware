using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Middleware_CoreWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            _ = new JWTUpload().UpdateRole();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}