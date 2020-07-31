using BLawSimulation.Models;
using BLawSimulation.Utils;
using MathNet.Numerics.Distributions;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace BLaw
{
    public class Results
    {
        public DirectoryInfo doBase = new DirectoryInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\");

        public FileInfo foTaux = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\results-taux.csv");
        public FileInfo foData = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\results-data.csv");
        public FileInfo foDataLarge = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\results-data-large.csv");
        public FileInfo foTauxGroup = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\results-taux-group.csv");
        public FileInfo forRawPool = new FileInfo(@"C:\Users\xavie\Desktop\BLawCode\Output\results-data-group.csv");


        // const int N = 1000; // Debug
        const int N = 100000; // Final


        double[] damageJitter = new double[N];

        List<double> multipliers = new List<double>();
        List<double> multipliersLarge = new List<double>();        

        List<double> multipliersTaux = new List<double>()
        {
            1000, 1200, 1500, 1800, 2200, 2700, 3300, 3900, 4700, 5600, 6800, 8200,
            10000
        };

        [SetUp]
        public void Setup()
        {
            // Debug
            // Random rnd = new Random(0); // for repetability
            // Triangular.Samples(rnd, damageJitter, 0.9, 1.1, 1.0);            
            Triangular.Samples(damageJitter, 0.9, 1.1, 1.0);

            for (int i = 5000; i < 25000; i += 2500)
                multipliersLarge.Add(i);

            for (int i = 500; i < 3500; i += 500)
                multipliers.Add(i);
        }

        [Test]
        public void CalculTauxProcedure()
        {
            var dt = new DataTable("choice");
            dt.Columns.Add("Facteur", typeof(double));
            dt.Columns.Add("TauxProces", typeof(double));
            dt.Columns.Add("Type", typeof(string));

            foreach (var m in multipliersTaux)
            {
                int count = 0, countSans = 0, countAvec = 0, countDemi = 0;
                for (int i = 1; i <= N; i++)
                {
                    count++;
                    var l = new Litige(m);
                    if (l.decidePremiereInstance())
                        countAvec++;
                    if (l.decidePremiereInstanceSansAvocat())
                        countSans++;
                    if (l.decidePremiereInstanceDemiAvance())
                        countDemi++;
                }
                System.Console.WriteLine("Finished set {0} ; total {1} simulations", m, count);
                dt.Rows.Add(m, ((double)countSans) / N, "CPC sans avocat");
                dt.Rows.Add(m, ((double)countAvec) / N, "CPC avec avocat");
                dt.Rows.Add(m, ((double)countDemi) / N, "P-CPC avec avocat");
            }
            var foTaux = new FileInfo(Path.Combine(doBase.FullName, "results-taux.csv"));
            Csv.Write(dt, foTaux);
        }

        [Test]
        public void GenerateCasesLarge()
        {
            var dtRaw = new DataTable("allData");
            dtRaw.Columns.Add("Facteur", typeof(double));
            dtRaw.Columns.Add("Dommage", typeof(double));
            dtRaw.Columns.Add("Frais", typeof(double));
            dtRaw.Columns.Add("Depens", typeof(double));
            dtRaw.Columns.Add("Honoraires", typeof(double));
            dtRaw.Columns.Add("ProbaGain", typeof(double));

            foreach (var m in multipliersLarge)
            {
                for (int i = 1; i <= N; i++)
                {
                    var l = new Litige(Math.Round(1 * m));
                    l.decidePremiereInstance();
                    dtRaw.Rows.Add(
                        m,
                        l.dommage,
                        l.fraisJudiciaires,
                        l.depens,
                        l.honoraireAvocats,
                        l.calculeProbabiliteMinimum()
                        );
                }
            }
            Csv.Write(dtRaw, foDataLarge);

        }

        [Test]
        public void GenerateCasesPool()
        {
            Dictionary<double, List<Litige>> dictLitige = new Dictionary<double, List<Litige>>();

            var dtRaw = new DataTable("allData");
            dtRaw.Columns.Add("Facteur", typeof(double));
            dtRaw.Columns.Add("Dommage", typeof(double));
            dtRaw.Columns.Add("Frais", typeof(double));
            dtRaw.Columns.Add("Depens", typeof(double));
            dtRaw.Columns.Add("Honoraires", typeof(double));
            dtRaw.Columns.Add("ProbaGain", typeof(double));

            var dtTauxPool = new DataTable("choicePool");
            dtTauxPool.Columns.Add("Dommage", typeof(double));
            dtTauxPool.Columns.Add("TauxProces", typeof(double));
            dtTauxPool.Columns.Add("Type", typeof(string));

            var dtRawPool = new DataTable("allDataPool");
            dtRawPool.Columns.Add("Facteur", typeof(double));
            dtRawPool.Columns.Add("FacteurGroup", typeof(double));
            dtRawPool.Columns.Add("Frais", typeof(double));
            dtRawPool.Columns.Add("Depens", typeof(double));
            dtRawPool.Columns.Add("Honoraires", typeof(double));
            dtRawPool.Columns.Add("ProbaGain", typeof(double));
            dtRawPool.Columns.Add("TypeGroup", typeof(string));

            foreach (var m in multipliers)
            {
                dictLitige[m] = new List<Litige>();
                int count = 0;
                foreach (var ds in damageJitter)
                {
                    var l = new Litige(Math.Round(ds * m));
                    dictLitige[m].Add(l);
                    dtRaw.Rows.Add(
                        m,
                        ds * m,
                        l.fraisJudiciaires,
                        l.depens,
                        l.honoraireAvocats,
                        l.calculeProbabiliteMinimum()
                        );
                    if (l.decidePremiereInstanceDemiAvance())
                        count++;
                }
                dtTauxPool.Rows.Add(m, ((double)count) / N, "PCP/1");
            }
            Csv.Write(dtRaw, foData);

            // Part pooling
            var pooling = new List<int>() { 2, 4, 10, 50, 100 };

            foreach (var m in multipliers)
            {
                var shuffledLitiges = dictLitige[m].OrderBy(a => Guid.NewGuid()).ToList();
                foreach (var p in pooling)
                {
                    var groupType = String.Format("G-{0}", p);
                    int count = 0;
                    int n = N / p;
                    for (int i = 0; i < n; i++)
                    {
                        var test = shuffledLitiges.GetRange(i * p, p);
                        var group = new LitigeGroup(test);
                        if (group.decidePremiereInstanceDemiAvance())
                        {
                            count += group.count;
                        }
                        dtRawPool.Rows.Add(m,
                            m*p,
                            group.fraisJudiciaires,
                            group.depens,
                            group.honoraireAvocats,
                            group.calculeProbabiliteMinimum(),
                            groupType
                            );
                    }
                    dtTauxPool.Rows.Add(m, ((double)count) / N, groupType);
                }
            }
            Csv.Write(dtTauxPool, foTauxGroup);
            Csv.Write(dtRawPool, forRawPool);

        }
    }
}