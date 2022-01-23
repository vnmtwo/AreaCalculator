using AreaCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AreaCalculatorTests
{
    [TestClass]
    public class AreaCalculatorTest
    {
        [TestMethod]
        public void Constructor()
        {
            var ac = new AreaCalculator.AreaCalculator();

            CollectionAssert.AreEqual(ac.Constants, new List<string>() { "{PI}" });
            CollectionAssert.AreEqual(ac.MathFuncs, new List<string>() { "SQRT", "POW", "SIN", "COS" });
            CollectionAssert.AreEqual(ac.FormulaNames, new List<string>() { "CircleByR", "TriangleBySides" });
        }

        [TestMethod]
        public void AddFormula_Normal()
        {
            AreaCalculator.AreaCalculator ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);

            ac.AddFormula(name, "[A]+[B]+1.0");
            
            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B], [A]+[B]");

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]+{PI}, [A]+[B]+{PI}");

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]+{PI}, [A]+[B]");
        }

        [TestMethod]
        public void AddFormula_ArgumentNullException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);

            Assert.ThrowsException<ArgumentNullException>(
                () => ac.AddFormula(name, null));
        }

        [TestMethod]
        public void AddFormula_FormulaCompilationException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);

            Assert.ThrowsException<FormulaCompilationException>(
                () => ac.AddFormula(name, "[A]+[B])"));
        }

        [TestMethod]
        public void AddFormula_FormulaWithoutVariablesException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);

            Assert.ThrowsException<FormulaWithoutVariablesException>(
                () => ac.AddFormula(name, "3+4"));
        }

        [TestMethod]
        public void CalculateCircleByR_Normal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            double r = 1;
            var d = ac.Calculate("CircleByR", new double[] { r });

            Assert.AreEqual(d[0], Math.PI * r * r);
        }

        [TestMethod]
        public void CalculateTriangleBySides_Normal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            double a = 3, b = 4, c = 5, s = 6;
            var d = ac.Calculate("TriangleBySides", new double[] {a,b,c});
            var v = ac.Calculate("TriangleBySides", new double[] { a + 1, b + 1, c + 1 });

            Assert.AreEqual(d[0], s);
            Assert.AreEqual(d[1], 1.0);
            Assert.AreEqual(v[1], 0.0);
        }

        [TestMethod]
        public void Calculate_ArgumentException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = ac.FormulaNames[0];
            var vc = ac.GetFormulaVariables(name).Count;
            var d = GenerateArgs(vc + 1);

            Assert.ThrowsException<ArgumentException>(
                () => ac.Calculate(name, d));
        }

        [TestMethod]
        public void Calculate_ArgumentNullException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = ac.FormulaNames[0];
            var vc = ac.GetFormulaVariables(name).Count;
            var d = GenerateArgs(vc);

            Assert.ThrowsException<ArgumentNullException>(
                () => ac.Calculate(name, null));
        }

        [TestMethod]
        public void GetFormulaVariables_Normal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]");

            CollectionAssert.AreEqual(ac.GetFormulaVariables(name),
                new List<string>() { "[A]", "[B]" });
        }

        private string GenerateUniqueString(ICollection<string> collection)
        {
            string n = "";
            var t = true;
            var rand = new Random();

            while (t)
            {
                n = rand.Next().ToString();
                if (!collection.Contains(n))
                    t = false;
            }
            return n;
        }
        private double[] GenerateArgs(int count)
        {
            double[] a = new double[count];
            for (int i = 0; i < count; i++)
            {
                a[i] = i+1;
            }
            return a;
        }
    }
}
