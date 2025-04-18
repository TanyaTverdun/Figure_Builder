using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    internal class Circle_Circle : Circle_figure
    {
        public override FigureSubType subType { get { return FigureSubType.Коло; } }
        public override string color { get; set; }
        public override double Area { get { return Math.Round(area(radius_R, radius_R), 3); } }

        // Default constructor
        public Circle_Circle() { }
        //Constructor with parameters
        public Circle_Circle(int w, int h, double R, string color)
        {
            width = w;
            height = h;
            radius_R = R;
            this.color = color;
        }
        // Copy constructor
        public Circle_Circle(Circle_Circle other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            width = other.width;
            height = other.height;
            radius_R = other.radius_R;
        }
        // Writing to a file
        public override void writeToFile(string fileName)
        {
            System.IO.File.AppendAllText(fileName, "Тип фігури: " + type + "\n");
            System.IO.File.AppendAllText(fileName, "Підтип фігури: " + subType + "\n");
            System.IO.File.AppendAllText(fileName, "Колір фігури: " + color + "\n");
            System.IO.File.AppendAllText(fileName, "Площа фігури: " + Math.Round(area(radius_R, radius_R), 3).ToString() + "\n");
            System.IO.File.AppendAllText(fileName, "Радіус кола: " + radius_R + "\n\n\n");
        }
        // Converting a class to an array of strings
        public override string[] convertToArray()
        {
            string[] strings = {
                type.ToString(),
                subType.ToString(),
                color,
                "Nan",
                "Nan",
                "Nan",
                "Nan",
                "Nan",
                "Nan",
                "Nan",
                "Nan",
                radius_R.ToString(),
                "Nan",
                perimeter().ToString(),
                Math.Round(area(radius_R, radius_R), 3).ToString(),
                "Nan",
                "Nan",
                middleLine().ToString()
            };
            return strings;
        }
    }
}
