using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    internal class Triangle_Isosceles : Triangle
    {
        public override FigureSubType subType { get { return FigureSubType.Рівнобедрений; } }
        public override string color { get; set; }
        public override double Area { get { return Math.Round(area(sideA, sideB, sideC), 3); } }

        // Default constructor
        public Triangle_Isosceles() { }
        //Constructor with parameters
        public Triangle_Isosceles(int w, int h, double A, double B, double C, string color)
        {
            width = w; 
            height = h;
            sideA = A;
            sideB = B;
            sideC = C;
            angleA = toGrad(Math.Acos((sideB * sideB + sideC * sideC - sideA * sideA) / (2 * sideB * sideC)));
            angleB = toGrad(Math.Acos((sideA * sideA + sideC * sideC - sideB * sideB) / (2 * sideA * sideC)));
            angleC = toGrad(Math.Acos((sideA * sideA + sideB * sideB - sideC * sideC) / (2 * sideA * sideB)));
            this.color = color;
        }
        // Copy constructor
        public Triangle_Isosceles(Triangle_Isosceles other)
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
            angleA = other.angleA;
            angleB = other.angleB;
            angleC = other.angleC;
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
            System.IO.File.AppendAllText(fileName, "Кут А: " + Math.Round(angleA, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Кут B: " + Math.Round(angleB, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Кут C: " + Math.Round(angleC, 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Периметр фігури: " + Math.Round(perimeter(sideA, sideB, sideC), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Площа фігури: " + Math.Round(area(sideA, sideB, sideC), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Радіус описаного кола: " + Math.Round(R(sideA, sideB, sideC), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Радіус вписаного кола: " + Math.Round(r(sideA, sideB, sideC), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Середня лінія: " + Math.Round(middleLine(sideA, sideB, sideC), 3) + "\n\n\n");
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
                "Nan",
                Math.Round(angleA, 3).ToString(),
                Math.Round(angleB, 3).ToString(),
                Math.Round(angleC, 3).ToString(),
                "Nan",
                "Nan",
                "Nan",
                Math.Round(perimeter(sideA, sideB, sideC), 3).ToString(),
                Math.Round(area(sideA, sideB, sideC), 3).ToString(),
                Math.Round(R(sideA, sideB, sideC), 3).ToString(),
                Math.Round(r(sideA, sideB, sideC), 3).ToString(),
                Math.Round(middleLine(sideA, sideB, sideC), 3).ToString()
            };
            return strings;
        }
    }
}
