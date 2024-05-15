using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Setup.Services
{
    public class SetupService
    {
        public void HealthCheckProjectInstallation()
        {
            try
            {
                //mongodb dosyasının yolunu al, Items/Mongodb dosyasının yolunu al, userprofile olmadan solution'un içindeki Items/Mongodb dosyasının yolunu al

                string mongoDbPath = Directory.GetCurrentDirectory(); // ilk değerini al
                while (!mongoDbPath.EndsWith("ServicesHealthCheck"))
                {
                    mongoDbPath = Directory.GetParent(mongoDbPath).FullName;
                }
                //setup project'ini içinden çıkar
                string mongoDbServerFile = "mongodb-windows-x86_64-*-signed.msi";
                //string mongoDbCompassFile = "mongodb-windows-x86_64-6.0.15-signed.msi";
                //mongodb dosyasını bul

                mongoDbPath = Path.Combine(mongoDbPath, "Items", "MongoDb");

                string[] mongoDbServerFiles = Directory.GetFiles(mongoDbPath, mongoDbServerFile);
                //string[] mongoDbCompassFiles = Directory.GetFiles(mongoDbPath, mongoDbCompassFile);

                if (mongoDbServerFiles.Length == 0)
                {
                    Console.WriteLine("Mongodb dosyası bulunamadı.");
                    return;
                }

                string mongoDbFile = mongoDbServerFiles[0];
                mongoDbFile = Path.GetFileName(mongoDbFile);
                //string mongoDbCompass = mongoDbCompassFiles[0];

                Console.WriteLine("Mongodb dosyası bulundu. Kurulum işlemi başlatılıyor.");

                //string mongodbKomut = $"msiexec.exe /l*v mdbinstall.log  /qb /i mongodb-windows-x86_64-4.4.29-signed.msi ^\r\n            ADDLOCAL=\"ServerService,Client\" ^\r\n            SHOULD_INSTALL_COMPASS=\"0\"";
                string arguments = $@"msiexec.exe /l*v mdbinstall.log /qb /i {mongoDbFile} ADDLOCAL=""ServerService,Client"" SHOULD_INSTALL_COMPASS=""0""";
                ProcessStartInfo startMongo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + arguments,
                    Verb = "runas",
                    WorkingDirectory = mongoDbPath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = startMongo
                };

                process.Start();

                // Wait for the process to finish.
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    //Console.WriteLine("Compass kuruluyor.");

                    //string compassKomut = $"msiexec.exe /l*v mdbinstall.log  /qb /i {mongoDbCompass}";

                    //ProcessStartInfo startCompass = new ProcessStartInfo
                    //{
                    //    FileName = "cmd.exe",
                    //    Arguments = "/c " + compassKomut,
                    //    Verb = "runas",
                    //    WorkingDirectory = mongoDbPath,
                    //    RedirectStandardOutput = true,
                    //    UseShellExecute = false,
                    //    CreateNoWindow = true
                    //};

                    //Process compassProcess = new Process();
                    //compassProcess.StartInfo = startCompass;
                    //compassProcess.Start();
                    //compassProcess.WaitForExit();

                    //if (compassProcess.ExitCode == 0)
                    //{
                    //    Console.WriteLine("Compass başarıyla yüklendi.");
                    //    Console.WriteLine("Mongodb başarıyla yüklendi.");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Compass yüklenirken bir hata oluştu.");
                    //    Console.WriteLine("Mongodb sunucuları yüklendi.");
                    //}

                    Console.WriteLine("Uygulama kuruluyor.");

                    string projectPath = Directory.GetCurrentDirectory(); // ilk değerini al

                    while (!projectPath.EndsWith("ServicesHealthCheck"))
                    {
                        projectPath = Directory.GetParent(projectPath).FullName;
                    }

                    projectPath = Path.Combine(projectPath, "HealthCheck.Presentation", "HealthCheck.Admin", "ServicesHealthCheck.Monitoring", "bin", "Release", "net8.0", "ServicesHealthCheck.Monitoring.exe");

                    if (!File.Exists(projectPath))
                    {
                        Console.WriteLine(
                            "Uygulama kurulumu için gerekli publish edilmiş .exe dosyası bulunamadı. Uygulama kurulamadı.");
                        return;
                    }

                    string projectKomut = $"sc create HealthCheckProject binpath= \"{projectPath}\" start=auto";

                    ProcessStartInfo startProject = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + projectKomut,
                        Verb = "runas",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process projectProcess = new Process();
                    projectProcess.StartInfo = startProject;
                    projectProcess.Start();
                    projectProcess.WaitForExit();

                    if (projectProcess.ExitCode == 0)
                    {
                        Console.WriteLine("Uygulama başarıyla kuruldu.");
                        ServiceController service = new ServiceController("HealthCheckProject");
                        service.Start();
                    }
                    else
                    {
                        Console.WriteLine("Uygulama yüklenirken bir hata oluştu. Uygulama kurulamadı.");
                    }
                    // Read the output of the process.
                    string outputProject = projectProcess.StandardOutput.ReadToEnd();
                    // Display the output of the process.
                    Console.WriteLine(outputProject);
                }
                else
                {
                    Console.WriteLine("Mongodb yüklenirken bir hata oluştu. Uygulama kurulamadı.");
                    // Read the output of the process.
                    string outputProject = process.StandardOutput.ReadToEnd();
                    // Display the output of the process.
                    Console.WriteLine(outputProject);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Bir hata oluştu. Uygulama kurulamadı. " + exception);
            }
        }
    }
}
