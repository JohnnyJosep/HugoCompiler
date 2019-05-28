using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace HugoCompiler
{
    public class HugoService
    {
        private static string HugoCompiler => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            $@"{Directory.GetCurrentDirectory()}\hugo\hugo.exe" :
            $@"{Directory.GetCurrentDirectory()}\hugo\hugo";

        public string Compile(string folderContent)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = HugoCompiler,
                    Arguments = $"-s {folderContent}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            StringBuilder builder = new StringBuilder();
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                builder.AppendLine(line);
            }

            return builder.ToString();
        }
    }
}
