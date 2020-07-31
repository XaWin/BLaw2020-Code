using BLawSimulation.Models;
using BLawSimulation.RandomGenerator;
using BLawSimulation.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Simulation
{

    public class Tests
    {
        public double maxVL = 10000000;
        public FileInfo fo = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\fees.csv");
        public FileInfo fo2 = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\dist-test.csv");
        public FileInfo foRes = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\res-sim.csv");

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GenerateDataFees()
        {
            var dt = new DataTable("feeTable");
            dt.Columns.Add("ValeurLitigieuse", typeof(double));
            dt.Columns.Add("FraisVaud", typeof(double));
            dt.Columns.Add("FraisNeuchatel", typeof(double));

            for (int i = 1000; i <= maxVL; i += 1000)
            {
                dt.Rows.Add(i,
                    FraisJudiciaire.estimateFeeVD(i),
                    FraisJudiciaire.estimateFeeNE(i)
                    );
            }
            Csv.Write(dt, fo);
        }

        [Test]
        public void GenerateDistPoints()
        {
            var N = 100000;
            var dt = new DataTable("data");
            dt.Columns.Add("trig", typeof(double));

            for (int i = 1; i < N; i++)
            {
                dt.Rows.Add(
                    Triangular.Random(10, 50, 15)
                    );
            }
            Csv.Write(dt, fo2);
        }

        [Test]
        public void GenerateCase()
        {
            var N = 100000;
            var dt = new DataTable("data");
            dt.Columns.Add("valueInDispute", typeof(double));
            dt.Columns.Add("isExtreme", typeof(bool));
            dt.Columns.Add("courtCost", typeof(double));
            dt.Columns.Add("attorneyCost", typeof(double));
            dt.Columns.Add("goToCourt", typeof(bool));

            var test = new CaseFactory();
            for (int i = 1; i < N; i++)
            {
                var newCase = test.createCase();
                newCase.calculateCost();

                dt.Rows.Add(
                    newCase.valueInDispute,
                    newCase.isExtreme,
                    newCase.courtCost,
                    newCase.attorneyCost,
                    newCase.decideCourt()
                    );
            }
            Csv.Write(dt, fo2);
        }
    }
}