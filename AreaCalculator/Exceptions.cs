using System;

namespace AreaCalculator
{
    public class FormulaCompilationException : Exception
    {
        public FormulaCompilationException() : base() { }
        public FormulaCompilationException(string message) : base(message) { }
    }

    public class FormulaWithoutVariablesException : Exception
    {
        public FormulaWithoutVariablesException() : base() { }
        public FormulaWithoutVariablesException(string message) : base(message) { }
    }
}
