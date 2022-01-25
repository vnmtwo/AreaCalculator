using AreaCalculator;

namespace AreaCalculatorTests.TestCalculators
{
    internal class TestAreaCalculator_CheckOrder: ICalculateArea
    {
        [FormulaField]
        public double c;
        [FormulaField]
        public double b;
        [FormulaField]
        public double a;

        public double[] CalculateArea(double[] args)
        {
            a = args[0];
            b = args[1];
            c = args[2];

            return new double[] { c,a,b };
        }
    }
}
