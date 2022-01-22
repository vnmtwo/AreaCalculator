using AreaCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AreaCalculatorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CalculateCircleByR_NormalTest()
        {
            AreaCalculator.AreaCalculator ac = new AreaCalculator.AreaCalculator();
            double r = 1;
            var d = ac.Calculate("CircleByR", new double[] { r });

            Assert.AreEqual(d[0], Math.PI * r * r);
        }

        [TestMethod]
        public void AddFormula_NormalTest()
        {
            AreaCalculator.AreaCalculator ac = new AreaCalculator.AreaCalculator();
            ac.AddFormula("CircleByD", "{PI}*POW([d]/2,2)");
        }
    }
}
