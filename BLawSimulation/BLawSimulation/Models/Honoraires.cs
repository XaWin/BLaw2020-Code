using MathNet.Numerics.Distributions;
using MathNet.Numerics.Optimization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.Models
{
    public class Honoraires
    {
        public static double estimateHonoraires(double valeurLitigieuse)
        {
            double min = 5 * 180 * 1.05 * 1.077; // 5h au tarif AJ, + débours et TVA
            double max = (61.38 / 100) * valeurLitigieuse;
            if (max < min)
                return min; // case where valeurLitigieuse is so small
            else 
                return Triangular.Sample(min, max, (max + min)/2);
        }

        public static double estimateHonorairesAvecConcurrence(double valeurLitigieuse, int count = 3)
        {
            double bestOffer = double.MaxValue;
            for (int i = 0; i < count  ; i++)
            {
                double newOffer = estimateHonoraires(valeurLitigieuse);
                bestOffer = newOffer < bestOffer ? newOffer : bestOffer;
            }
            return bestOffer;
        }
    }
}
