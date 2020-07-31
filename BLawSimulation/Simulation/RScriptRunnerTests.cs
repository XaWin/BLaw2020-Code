using BLawSimulation;
using BLawSimulation.Models;
using BLawSimulation.RandomGenerator;
using MathNet.Numerics.Statistics;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilitiesTests
{
    public class RScriptRunnerTests
    {

        [Test]
        public void GenerateDataFees()
        {
            var t = RScriptCaller.GenerateVisual();
            System.Console.WriteLine(t);
        }


        [Test]
        public static void TestRscriptExe()
        {            
            ProcessStartInfo info = new ProcessStartInfo("rscript.exe");
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            string result = string.Empty;

            using (var proc = new Process())
            {
                proc.StartInfo = info;
                proc.Start();
                result = proc.StandardOutput.ReadToEnd();
                proc.CloseMainWindow();
                proc.Close();
            }
            System.Console.WriteLine(result);
        }

        [Test]
        public static void SimpleProcess()
        {
            Process.Start("rscript.exe");
        }
}
}