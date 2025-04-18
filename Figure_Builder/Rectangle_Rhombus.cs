using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    internal class Rectangle_Rhombus : Rectangle_figure
    {
        public override FigureSubType subType { get { return FigureSubType.Ромб; } }
        public override string color { get; set; }
        public override double Area { get { return Math.Round(area(sideA, sideB, angleA), 3); } }

        // Default constructor
        public Rectangle_Rhombus() { }
        //Constructor with parameters
        public Rectangle_Rhombus(int w, int h, double side, double d1, double d2, string color)
        {
            width = w;
            height = h;
            sideA = sideB = sideC = sideD = side;
            angleA = angleC = toGrad(Math.Acos((d1 * d1 + d2 * d2 - 4 * side * side) / (2 * d1 * d2)));
            angleB = angleD = 180 - angleA;
            this.color = color;
        }
        // Copy constructor
        public Rectangle_Rhombus(Rectangle_Rhombus other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            width = other.width;
            height = other.height;
            sideA = other.sideA;
            sideB = other.sideB;
            sideC = other.sideC;
            sideD = other.sideD;
            angleA = other.angleA;
            angleB = other.angleB;
            angleC = other.angleC;
            angleD = other.angleD;
        }
        protected double R()
        {
            return Double.NaN;
        }

        // Calculate the radius of an inscribed circle in a figure
        protected double r(double side, double angle)
        {
            return (area(side, side, angle) / (2 * side));
        }
        // Writing to a file
        public override void writeToFile(string fileName)
        {
            System.IO.File.AppendAllText(fileName, "Тип фігури: " + type + "\n");
            System.IO.File.AppendAllText(fileName, "Підтип фігури: " + subType + "\n");
            System.IO.File.AppendAllText(fileName, "Колір фігури: " + color + "\n");
            System.IO.File.AppendAllText(fileName, "Сторона А: " + Math.Round(sideA, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Сторона B: " + Math.Round(sideB, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Сторона C: " + Math.Round(sideC, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Сторона D: " + Math.Round(sideD, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Кут А: " + Math.Round(angleA, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Кут B: " + Math.Round(angleB, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Кут C: " + Math.Round(angleC, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Кут D: " + Math.Round(angleD, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Периметр фігури: " + Math.Round(perimeter(sideA, sideB, sideC, sideD), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Площа фігури: " + Math.Round(area(sideA, sideB, angleA), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Радіус описаного кола: " + R() + "\n");
            System.IO.File.AppendAllText(fileName, "Радіус вписаного кола: " + Math.Round(r(sideA, sideA), 3) + "\n\n\n");
        }
        // Converting a class to an array of strings
        public override string[] convertToArray()
        {
            string[] strings = {
                type.ToString(),
                subType.ToString(),
                color,
                Math.Round(sideA, 3).ToString(),
                Math.Round(sideB, 3).ToString(),
                Math.Round(sideC, 3).ToString(),
                Math.Round(sideD, 3).ToString(),
                Math.Round(angleA, 3).ToString(),
                Math.Round(angleB, 3).ToString(),
                Math.Round(angleC, 3).ToString(),
                Math.Round(angleD, 3).ToString(),
                "Nan",
                "Nan",
                Math.Round(perimeter(sideA, sideB, sideC, sideD), 3).ToString(),
                Math.Round(area(sideA, sideB, angleA), 3).ToString(),
                R().ToString(),
                Math.Round(r(sideA, angleA),3).ToString(),
                middleLine().ToString()
            };
            return strings;
        }
    }
}
