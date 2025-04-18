using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    internal class Triangle : Figures
    {
        public override FigureType type { get { return FigureType.Трикутник; } }

        public double sideA { get; set; }
        public double sideB { get; set; }
        public double sideC { get; set; }
        protected double angleA;
        protected double angleB;
        protected double angleC;

        // Perimeter calculation
        protected double perimeter(double sideA, double sideB, double sideC)
        {
            return (sideA + sideB + sideC);
        }
        // Area calculation
        protected double area(double sideA, double sideB, double sideC)
        {
            double p = perimeter(sideA, sideB, sideC) / 2;
            return (Math.Sqrt(p * (p - sideA) * (p - sideB) * (p - sideC)));
        }
        // Calculation of the middle line
        protected double middleLine(double sideA, double sideB, double sideC)
        {
            return (sideC / 2);
        }
        // Calculating the radius of the circumscribed circle around a figure
        protected double R(double sideA, double sideB, double sideC)
        {
            return ((sideA * sideB * sideC) / (4 * area(sideA, sideB, sideC)));
        }
        // Calculate the radius of an inscribed circle in a figure
        protected double r(double sideA, double sideB, double sideC)
        {
            return (area(sideA, sideB, sideC) / (perimeter(sideA, sideB, sideC) / 2));
        }
        // Converting radians to degrees
        protected double toGrad(double rad)
        {
            return (rad * (180 / Math.PI));
        }
    }
}
