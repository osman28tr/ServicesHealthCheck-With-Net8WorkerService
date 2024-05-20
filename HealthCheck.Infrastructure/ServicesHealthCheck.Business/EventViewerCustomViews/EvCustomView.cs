using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Serilog;
using ServicesHealthCheck.Business.EventViewerCustomViews.Abstract;

namespace ServicesHealthCheck.Business.EventViewerCustomViews
{
    public class EvCustomView : IEvCustomView
    {
        public Task CreateCustomViewAsync(List<string> services)
        {
            try
            {
                string filePath = @"C:\ProgramData\Microsoft\Event Viewer\Views\HealthCheckCustomView.xml"; //görünümün kaydedileceği dosya yolu
                
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.Unicode))
                {
                    writer.Formatting = Formatting.Indented;

                    // Kök eleman
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ViewerConfig"); // xml'de ViewerConfig elemanının oluşturulması

                    // QueryConfig elemanı
                    writer.WriteStartElement("QueryConfig");

                    // QueryParams elemanı
                    writer.WriteStartElement("QueryParams");
                    writer.WriteStartElement("UserQuery");
                    writer.WriteEndElement(); // UserQuery
                    writer.WriteEndElement(); // QueryParams

                    writer.WriteStartElement("QueryNode");
                    writer.WriteElementString("Name", "HealthCheckView"); // QueryNode elemanı içinde Name elemanının oluşturulması ve değer atanması

                    // Querylerin girilmesi, ilgili servislerin filtrelenmesi
                    writer.WriteStartElement("QueryList");

                    writer.WriteStartElement("Query");
                    writer.WriteAttributeString("Id", "0"); //Query elemanına Id özelliği eklenmesi ve değer atanması
                    writer.WriteAttributeString("Path", "Application");

                    //servislere göre filtreleme
                    foreach (var service in services)
                    {
                        writer.WriteStartElement("Select");
                        writer.WriteAttributeString("Path", "Application");
                        writer.WriteString($"*[System[Provider[@Name='{service}'] and (Level=2 or Level=3 or Level=4)]]");
                        writer.WriteEndElement(); // Select
                    }
                    writer.WriteEndElement(); // Query
                    writer.WriteEndElement(); // QueryList
                    writer.WriteEndElement(); // QueryNode
                    writer.WriteEndElement(); // QueryConfig
                    writer.WriteEndElement(); // ViewerConfig

                    writer.WriteEndDocument();
                }

                Log.Information($"Custom view XML file has been created at: {filePath}");

                // PowerShell komutu oluştur
                string psCommand = $"wevtutil im \"{filePath}\""; // wevtutil komutu ile oluşturulan xml dosyasının eventviewer içerisine import edilmesi

                // PowerShell komutunu çalıştır
                ExecuteCustomViewCommand(psCommand);

                Log.Information("Custom view has been imported into Event Viewer under Custom Views.");

            }
            catch (XmlException ex)
            {
                Log.Error(ex.StackTrace + ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace + ex.Message);
            }

            return Task.CompletedTask;
        }

        private void ExecuteCustomViewCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = "cmd.exe";
            processInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"";
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Output: " + output);
            }

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error: " + error);
            }
        }
    }
}
