using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLawSimulation.Models
{
    public class CaseFactory
    {
        Normal normalDamage;
        ContinuousUniform extremeDirac = new ContinuousUniform(0, 1);
        Weibull extremeDamageMultiplier = new Weibull(1, 1.5);

        // 
        double meanDamage = 5000;
        double stdDamage = 1000;
        //
        double extremeProb = 0.01; // 1%
        double extremeDamageValue = 100000;

        public CaseFactory()
        {
            normalDamage = new Normal(meanDamage, stdDamage);            
        }

        public Case createCase()
        {
            var newCase = new Case();

            // Generate positive new damage
            var damage = normalDamage.Sample();
            while (damage < 0)
                damage = normalDamage.Sample();
            // Add an extreme event in some cases
            bool isExtreme = extremeDirac.Sample() < extremeProb;
            if (isExtreme)
                damage += extremeDamageValue * extremeDamageMultiplier.Sample();
            // Create cases
            newCase.setDamage(Math.Floor(damage), isExtreme);
            return newCase;
        }
    }
    public class Case
    {
        Guid caseId = Guid.NewGuid();
        LocationEnum location = LocationEnum.Vaud;

        bool hasProtJuri;
        bool hasAssJudi;

        public bool isExtreme { get; private set; }
        public double valueInDispute { get; private set; }

        public double courtCost { get; private set; }

        public double attorneyCost { get; private set; }

        public void setDamage(double damage)
        {
            valueInDispute = damage;
        }
        public void setDamage(double damage, bool extremFlag)
        {
            valueInDispute = damage;
            isExtreme = extremFlag;
        }

        public void calculateCost()
        {
            calculateCourtCost();
            attorneyCost = 3000;
        }

        public void calculateCourtCost()
        {
            courtCost = FraisJudiciaire.estimateCourtCost(valueInDispute, location);
        }

        public bool decideCourt()
        {
            if (courtCost + attorneyCost > valueInDispute)
                return false;
            else
                return true;
        }
    }
}