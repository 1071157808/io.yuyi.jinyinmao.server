using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagasTransfer
{
    public static class FileHelper
    {
        public static void WriteTo(string baseDir,string msg)
        {
            string dir = Path.Combine(baseDir, "log");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = $"{DateTime.UtcNow.AddHours(8).ToString("yyyyMMdd")}.txt";
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(dir, fileName), FileMode.Append)))
            {
                sw.WriteLine("--------------------------------Error--------------------------------");
                sw.WriteLine($"Time:{DateTime.UtcNow.AddHours(8).ToLongDateString()} ");
                sw.WriteLine(msg);
                sw.WriteLine("--------------------------------Error--------------------------------");
            }
        }
    }
}
