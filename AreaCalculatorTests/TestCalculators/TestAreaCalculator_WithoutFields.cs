using AreaCalculator;

namespace AreaCalculatorTests.TestCalculators
{
    internal class TestAreaCalculator_WithoutFields : ICalculateArea
    {
        public double[] CalculateArea(double[] args)
        {
            double a = args[0];
            double h = args[1];

            return new double[] { 0.5 * a * h };
        }
    }
}
