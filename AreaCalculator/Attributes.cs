using System;

namespace AreaCalculator
{
    /// <summary>
    /// Indicates a field with a variable in an implementing ICalculateArea class
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FormulaFieldAttribute : System.Attribute
    {

    }
}