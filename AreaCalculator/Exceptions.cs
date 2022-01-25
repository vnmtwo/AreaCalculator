using System;

namespace AreaCalculator
{

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    
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

    public class ClassNoContainsFormulaFieldsException : Exception
    {
        public ClassNoContainsFormulaFieldsException() : base() { }
        public ClassNoContainsFormulaFieldsException(string message) : base(message) { }
    }
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена

}
