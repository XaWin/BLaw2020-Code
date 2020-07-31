using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.Models
{
    class Depens
    {
        public static double estimationDepens(double valeurLitigieuse, LocationEnum location)
        {
            switch (location)
            {
                case LocationEnum.Vaud:
                    return estimationDepensVD(valeurLitigieuse);
                default:
                    throw new NotImplementedException("Pas d'estimateur des dépens pour cette localisation");
            }
        }
        public static double estimationDepensVD(double vl)
        {
            double dMin, dMax, vlMin, vlMax;
            if (vl < 0)
                throw new ArgumentOutOfRangeException("valeur litigieuse inférieure à zero");
            else if (vl >= 0 && vl < 30000)
            {
                dMin = 1000; dMax = 9000; vlMin = 0; vlMax = 30000;
            }
            else if (vl >= 30000 && vl < 100000)
            {
                dMin = 3000; dMax = 15000; vlMin = 30000; vlMax = 100000;
            }
            else if (vl >= 100000 && vl < 250000)
            {
                dMin = 6000; dMax = 25000; vlMin = 100000; vlMax = 250000;
            }
            else if (vl >= 250000 && vl < 500000)
            {
                dMin = 9000; dMax = 40000; vlMin = 250000; vlMax = 500000;
            }
            else
                throw new NotImplementedException(String.Format("Pas de formule pour les dépens pour VD pour VL={0}", vl));

            double dMode = dMin + 0.5*(dMax - dMin) * (vl-vlMin) / (vlMax-vlMin);
            return Triangular.Sample(dMin, dMax, dMode);
        }
    }
}