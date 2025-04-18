using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    class Rectangle_figure : Figures
    {
        public override FigureType type { get { return FigureType.Чотирикутник; } }
        public double sideA { get; set; }
        public double sideB { get; set; }
        public double sideC { get; set; }
        public double sideD { get; set; }
        protected double angleA;
        protected double angleB;
        protected double angleC;
        protected double angleD;
        public override double Area { get; }
        // Perimeter calculation
        protected double perimeter(double sideA, double sideB, double sideC, double sideD)
        {
            return sideA + sideB + sideC + sideD;
        }
        // Area calculation
        protected double area(double sideA, double sideB, double ang)
        {
            return (sideA * sideB * Math.Sin(ang*Math.PI/180));
        }
        protected double middleLine()
        {
            return Double.NaN;
        }
        // Converting radians to degrees
        protected double toGrad(double rad)
        {
            return (rad * (180 / Math.PI));
        }
    }
}
