using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.Models
{
    public class Litige
    {
        public double dommage { get; protected set; }
        protected LocationEnum canton = LocationEnum.Vaud;

        public double honoraireAvocats { get; protected set; }
        public double fraisJudiciaires { get; protected set; }
        public double depens { get; protected set; }

        protected bool decision;
        protected double probaGainMinimum;

        public Litige() { }

        public Litige(double newDommage)
        {
            setDommage(newDommage);
        }

        public void setDommage(double newDommage)
        {
            dommage = newDommage;
            honoraireAvocats = Math.Round(Honoraires.estimateHonorairesAvecConcurrence(newDommage));
            fraisJudiciaires = Math.Round(FraisJudiciaire.estimateFeeConciliationVD(newDommage)
                + FraisJudiciaire.estimateCourtCost(newDommage, canton));
            depens = Math.Round(Depens.estimationDepens(newDommage, canton));
        }

        public void overrideHonoraire(double honoraires)
        {
            honoraireAvocats = honoraires;
        }

        public bool decidePremiereInstance()
        {
            double coutProces = fraisJudiciaires + honoraireAvocats;
            decision = dommage >= coutProces;
            return decision;
        }

        public bool decidePremiereInstanceSansAvocat()
        {
            double coutProces = fraisJudiciaires;
            decision = dommage >= coutProces;
            return decision;
        }

        public bool decidePremiereInstanceDemiAvance()
        {           
            double coutProces = fraisJudiciaires + honoraireAvocats - 0.5* FraisJudiciaire.estimateCourtCost(dommage, canton); ;
            decision = dommage >= coutProces;
            return decision;
        }

        public double calculeProbabiliteMinimum()
        {
            if (decision)
            {
                double numerator = honoraireAvocats + depens + fraisJudiciaires;
                double denominator = dommage + 2 * depens + fraisJudiciaires;
                probaGainMinimum = numerator / denominator;
            }
            else
            {
                probaGainMinimum = double.NaN;
            }
            return probaGainMinimum;
        }


    }
}
