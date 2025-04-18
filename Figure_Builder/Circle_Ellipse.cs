using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    internal class Circle_Ellipse : Circle_figure
    {
        public override FigureSubType subType { get { return FigureSubType.Овал; } }
        public override string color { get; set; }
        public override double Area { get { return Math.Round(area(radius_R, radius_r), 3); } }

        // Default constructor
        public Circle_Ellipse() { }
        //Constructor with parameters
        public Circle_Ellipse(int w, int h, double R, double r, string color)
        {
            width = w;
            height = h;
            radius_R = R;
            radius_r = r;
            this.color = color;
        }
        // Copy constructor
        public Circle_Ellipse(Circle_Ellipse other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            width = other.width;
            height = other.height;
            radius_R = other.radius_R;
            radius_r = other.radius_r;
        }
        // Writing to a file
        public override void writeToFile(string fileName)
        {
            System.IO.File.AppendAllText(fileName, "Тип фігури: " + type + "\n");
            System.IO.File.AppendAllText(fileName, "Підтип фігури: " + subType + "\n");
            System.IO.File.AppendAllText(fileName, "Колір фігури: " + color + "\n");
            System.IO.File.AppendAllText(fileName, "Площа фігури: " + Math.Round(area(radius_R, radius_r), 3) + "\n");
            System.IO.File.AppendAllText(fileName, "Великий радіус овалу: " + radius_R + "\n");
            System.IO.File.AppendAllText(fileName, "Малий радіус овалу: " + radius_r + "\n\n\n");
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
                radius_r.ToString(),
                perimeter().ToString(),
                Math.Round(area(radius_R, radius_r), 3).ToString(),
                "Nan",
                "Nan",
                middleLine().ToString()
            };
            return strings;
        }
    }
}
