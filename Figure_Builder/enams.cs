using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    // List of figures types
    public enum FigureType
    {
        Трикутник,
        Чотирикутник,
        Коло
    }
    // List of figures subtypes
    public enum FigureSubType
    {
        Різносторонній,
        Рівносторонній,
        Рівнобедрений,
        Прямокутний,

        Квадрат,
        Прямокутник,
        Паралелепіпед,
        Трапеція,
        Ромб,

        Коло,
        Овал,
    }
    public static class Constants
    {
        public const double DoublePrecision = 0.00001;
    }

}
