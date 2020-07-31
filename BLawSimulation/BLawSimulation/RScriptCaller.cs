using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BLawSimulation
{
    /// </summary>
    public static class RScriptCaller
    {
        /// <summary>
        /// Runs an R script from a file using Rscript.exe.
        /// Example:  
        ///   RScriptRunner.RunFromCmd(curDirectory + @"\ImageClustering.r", "rscript.exe", curDirectory.Replace('\\','/'));
        /// Getting args passed from C# using R:
        ///   args = commandArgs(trailingOnly = TRUE)
        ///   print(args[1]);
        /// </summary>
        /// <param name="rCodeFilePath">File where your R code is located.</param>
        /// <param name="rScriptExecutablePath">Usually only requires "rscript.exe"</param>
        /// <param name="args">Multiple R args can be seperated by spaces.</param>
        /// <returns>Returns a string with the R responses.</returns>
        public static string RunFromCmd(string rCodeFilePath, FileInfo workingDirectory, string args)
        {
            string file = rCodeFilePath;
            string result = string.Empty;

            try
            {

                var info = new ProcessStartInfo("rscript.exe");
                info.WorkingDirectory = workingDirectory.FullName;
                info.Arguments = rCodeFilePath + " " + args;

                info.RedirectStandardInput = false;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    result = proc.StandardOutput.ReadToEnd();
                    proc.CloseMainWindow();
                    proc.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("R Script failed: " + result, ex);
            }
        }

        public static string GenerateVisual()
        {            
            string rCodeFilePath = @"C:\Users\xavie\Desktop\BLawCode\BlawGraphicGenerator\GraphicalGenerator.R";
            FileInfo rWorkingDirectory = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\output");
            return (RunFromCmd(rCodeFilePath, rWorkingDirectory, ""));
        }
    }
}


