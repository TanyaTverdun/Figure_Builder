using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figure_Builder
{
    // Exception class for incorrect data
    internal class InvalidInputException : Exception
    {
        public InvalidInputException() { }
    }
    // Exception class for the case when data is not entered
    internal class NoDataException : Exception
    {
        public NoDataException(string message) :base(message) { }
    }
}
