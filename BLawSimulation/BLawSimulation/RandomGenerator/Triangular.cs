using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.RandomGenerator
{
    public static class Triangular
    {
        private static Random pRandom = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        // public static Random(double min, double max, double mode)
        public static double Random(double min, double max, double mode)
        {
            double Fc = (mode - min) / (max - min);
            double P = 1.0e-6 * (double)pRandom.Next(1000000);
            if (P > 0 && P < Fc)
            {
                return min + Math.Sqrt(P * (max - min) * (mode - min));
            }
            else
            {
                return max - Math.Sqrt((1 - P) * (max - min) * (max - mode));
            }
        }
    }
}
