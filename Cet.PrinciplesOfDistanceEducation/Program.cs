using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;

namespace Cet.PrinciplesOfDistanceEducation
{
    public class Program
    {
        public static void Main(string[] args)
        {
           Load();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static void Load()
        {
            FFmpeg.ExecutablesPath = Path.Combine(Environment.CurrentDirectory, "FFpeg");
            // await FFmpeg.GetLatestVersion();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
