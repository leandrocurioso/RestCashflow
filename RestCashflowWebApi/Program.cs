using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace RestCashflowWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseKestrel(options => {
                       options.AddServerHeader = false;
                   })
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>();
    }
}
