using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.Models
{
    public class FraisJudiciaire
    {
        public static double estimateCourtCost(double valeurLitigieuse, LocationEnum location)
        {
            switch (location)
            {
                case LocationEnum.Vaud:
                    return estimateFeeVD(valeurLitigieuse);
                case LocationEnum.Neuchatel:
                    return estimateFeeNE(valeurLitigieuse);
                default:
                    throw new NotImplementedException("Pas de valeur litigieuse pour cette localisation");
            }
        }

        public static double estimateFeeVD(double valeurLitigieuse)
        {
            if (valeurLitigieuse < 0)
                throw new ArgumentOutOfRangeException("valeur litigieuse inférieure à zero");
            else if (valeurLitigieuse >= 0 && valeurLitigieuse < 30000)
                return 3750.0;
            else if (valeurLitigieuse >= 30000 && valeurLitigieuse < 100000)
                return 7000.0;
            else if (valeurLitigieuse >= 100000 && valeurLitigieuse < 250000)
                return 9500.0;
            else if (valeurLitigieuse >= 250000 && valeurLitigieuse < 500000)
                return 11500.0;
            else
                return Math.Min(15500 + 0.015 * (valeurLitigieuse - 500000), 300000);
        }

        public static double estimateFeeConciliationVD(double valeurLitigieuse)
        {
            if (valeurLitigieuse < 0)
                throw new ArgumentOutOfRangeException("valeur litigieuse inférieure à zero");
            else if (valeurLitigieuse >= 0 && valeurLitigieuse < 2000)
                return 150;
            else if (valeurLitigieuse >= 2000 && valeurLitigieuse < 5000)
                return 210;
            else if (valeurLitigieuse >= 5000 && valeurLitigieuse < 10000)
                return 300;
            else if (valeurLitigieuse >= 10000 && valeurLitigieuse < 30000)
                return 360;
            else if (valeurLitigieuse >= 30000 && valeurLitigieuse < 100000)
                return 900;
            else
                return Math.Min(1200 + 0.0025 * (valeurLitigieuse - 500000), 5000);
        }

        public static double estimateFeeNE(double valeurLitigieuse)
        {
            if (valeurLitigieuse < 0)
                throw new ArgumentOutOfRangeException("valeur litigieuse inférieure à zero");
            else if (valeurLitigieuse >= 0 && valeurLitigieuse < 2000)
                return 500.0;
            else if (valeurLitigieuse >= 2000 && valeurLitigieuse < 5000)
                return 900.0;
            else if (valeurLitigieuse >= 5000 && valeurLitigieuse < 8000)
                return 1000.0;
            else if (valeurLitigieuse >= 8000 && valeurLitigieuse < 10000)
                return 1200;
            else if (valeurLitigieuse >= 10000 && valeurLitigieuse < 30000)
                return 0.13 * valeurLitigieuse;
            else if (valeurLitigieuse >= 30000 && valeurLitigieuse < 100000)
                return 4000 + 0.03 * (valeurLitigieuse - 30000);
            else if (valeurLitigieuse >= 100000 && valeurLitigieuse < 1000000)
                return 6500 + 0.03 * (valeurLitigieuse - 100000);
            else
                return Math.Min(0.04 * valeurLitigieuse, 300000);
        }
    }
}