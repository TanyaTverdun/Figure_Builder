using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    internal class Circle_figure :Figures
    {
        public override FigureType type { get { return FigureType.Коло; } }
        public double radius_R { get; set; }
        public double radius_r { get; set; }

        protected double perimeter()
        {
            return Double.NaN;
        }
        // Area calculation
        protected double area(double R, double r)
        {
            return (Math.PI * R * r);
        }
        protected double middleLine()
        {
            return Double.NaN;
        }
    }
}
