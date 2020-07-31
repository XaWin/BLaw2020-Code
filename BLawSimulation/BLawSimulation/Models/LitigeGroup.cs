using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.Models
{
    public class LitigeGroup : Litige
    {
        public int count { get; private set; }
        List<Litige> poolLitiges;

        public LitigeGroup(List<Litige> litiges)
        {
            poolLitiges = litiges;
            count = poolLitiges.Count;

            // Sommation des dommages & moyennes des honoraires
            double domageTotal = 0;
            foreach (var l in litiges)
            {
                domageTotal += l.dommage; // dommage cummulé
                honoraireAvocats += l.honoraireAvocats / count; // honoraires moyens
            }
            setDommage(domageTotal);
            overrideHonoraire(honoraireAvocats);
        }
        public new bool decidePremiereInstance()
        {
            double coutProces = 0.5 * fraisJudiciaires + honoraireAvocats;
            decision = dommage >= coutProces;
            return decision;
        }
    }
}
