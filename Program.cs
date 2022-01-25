using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;


namespace DownloadsFolderOrganizer_cSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<downloadFolderOrganizer>(s =>
                {
                    s.ConstructUsing(dfo => new downloadFolderOrganizer());
                    s.WhenStarted(dfo => dfo.Start());
                    s.WhenStopped(dfo => dfo.Stop());
                });
                x.RunAs("desktopname/username", "password");
                x.SetServiceName("DownloadFolderOrganizer");
                x.SetDisplayName("İndirilenleri Düzenleyici");
                x.SetDescription("Bu servis indirilen dosyaları türüne göre düzenliyor.");
            });
            int exitCodeDegeri = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeDegeri;
        }
    }
}
