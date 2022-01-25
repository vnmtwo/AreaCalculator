namespace AreaCalculator
{
    /// <summary>
    /// Implements ICalculate interface to calculate a quad area.
    /// </summary>
    public class QuadAreaCalculator : ICalculateArea
    {
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
        
        [FormulaField]
        public double a;

        [FormulaField]
        public double b;

#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена

        /// <summary>
        /// ICalculate implementation
        /// </summary>
        /// <param name="args">
        /// double[2] as input
        /// </param>
        /// <returns>
        /// double[1]{quadArea}
        /// </returns>
        public double[] CalculateArea(double[] args)
        {
            a = args[0];
            b = args[1];

            return new[] { a * b };
        }
    }
}
