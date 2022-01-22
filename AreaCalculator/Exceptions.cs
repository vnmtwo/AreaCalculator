using System;

namespace AreaCalculator
{
    internal class FormulaCompilationException : Exception
    {
        public FormulaCompilationException() : base() { }
        public FormulaCompilationException(string message) : base(message) { }
    }

    internal class FormulaWithoutVariablesException : Exception
    {
        public FormulaWithoutVariablesException() : base() { }
        public FormulaWithoutVariablesException(string message) : base(message) { }
    }
}
