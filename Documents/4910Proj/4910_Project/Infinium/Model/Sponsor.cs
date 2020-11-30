using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class Sponsor
    {

        private int id;
        private string name;
        private double usdToPoints;

        public Sponsor(int id, string name, double usdToPoints)
        {
            this.id = id;
            this.name = name;
            this.usdToPoints = usdToPoints;
        }

        public int GetId()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public double GetUsdToPoints()
        {
            return usdToPoints;
        }

        public void SetUsdToPoints(double usdToPoints)
        {
            this.usdToPoints = usdToPoints;
        }

    }
}