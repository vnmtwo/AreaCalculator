using AreaCalculator;

namespace AreaCalculatorTests.TestCalculators
{
    internal class TestAreaCalculator_ForgottenAttr : ICalculateArea
    {
        public double a;
        public double h;

        public double[] CalculateArea(double[] args)
        {
            a = args[0];
            h = args[1];

            return new double[] { 0.5 * a * h };
        }
    }
}
