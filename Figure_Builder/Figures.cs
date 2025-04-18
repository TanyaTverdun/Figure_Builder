using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Xml.Linq;

namespace Figure_Builder
{
    public class Figures
    {
        public virtual FigureType type { get; }
        public virtual FigureSubType subType { get; }
        public int width {  get; set; }
        public int height { get; set; }
        public virtual double Area { get; }
        public virtual string color { get; set; }
        // Writing to a file
        public virtual void writeToFile(string fileName) { }
        // Converting a class to an array of strings
        public virtual string[] convertToArray() { string[] str = new string[14]; return str; }
        // Override operator more
        public static bool operator >(Figures f1, Figures f2) {
            return f1.Area > f2.Area;
        }
        // Override operator less
        public static bool operator <(Figures f1, Figures f2) {
            return f1.Area < f2.Area;
        }
        // Operator override is less or equal 
        public static bool operator <=(Figures f1, Figures f2)
        {
            return f1.Area <= f2.Area;
        }
        // Operator override is more or equal 
        public static bool operator >=(Figures f1, Figures f2)
        {
            return f1.Area >= f2.Area;
        }
        // Operator override is equal 
        public static bool operator ==(Figures? figure1, Figures? figure2)
            => figure1?.Equals(figure2) ?? false;
        // // Operator override isn`t equal 
        public static bool operator !=(Figures? figure1, Figures? figure2)
            => !(figure1 == figure2);
        public override int GetHashCode()
        {
            return HashCode.Combine(type, subType, width, height, color);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is Figures figure)
            {
                return type == figure.type
                    && Math.Abs(figure.subType - subType) <= Constants.DoublePrecision
                       && figure.width == width
                       && figure.height == height
                       && figure.color == color;
            }
            return false;
        }

    }
}
