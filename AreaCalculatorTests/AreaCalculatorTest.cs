using AreaCalculator;
using AreaCalculatorTests.TestCalculators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            CollectionAssert.AreEqual(ac.FormulaNames, new List<string>() { "CircleByR", "TriangleBySides", "QuadByAB" });
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

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, new TestAreaCalculator_Normal());
        }

        [TestMethod]
        public void AddFormula_ArgumentNullException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            string nil = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => ac.AddFormula(name, nil));
            Assert.ThrowsException<ArgumentNullException>(
                () => ac.AddFormula(null, "[A]+[B]"));

            TestAreaCalculator_Normal nul = null;
            name = GenerateUniqueString(ac.FormulaNames);
            Assert.ThrowsException<ArgumentNullException>(
                () => ac.AddFormula(nil, new TestAreaCalculator_Normal()));
            Assert.ThrowsException<ArgumentNullException>(
                () => ac.AddFormula(name, nul));
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
        public void AddFormula_ClassNoContainsFormulaFieldsException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            Assert.ThrowsException<ClassNoContainsFormulaFieldsException>(
                () => ac.AddFormula(name, new TestAreaCalculator_WithoutFields()));

            name = GenerateUniqueString(ac.FormulaNames);
            Assert.ThrowsException<ClassNoContainsFormulaFieldsException>(
                () => ac.AddFormula(name, new TestAreaCalculator_ForgottenAttr()));
        }

        [TestMethod]
        public void GetFormulaVariables_ArgumentNullException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            Assert.ThrowsException<ArgumentNullException>(
                () => ac.GetFormulaVariables(null));
        }

        [TestMethod]
        public void GetFormulaVariables_KeyNotFoundException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);

            Assert.ThrowsException<KeyNotFoundException>(
                () => ac.GetFormulaVariables(name));
        }

        [TestMethod]
        public void Calculate_Normal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]");
            var d = ac.Calculate(name, new double[] { 1, 2 });
            Assert.AreEqual(d[0], 3);

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]+1.0");
            d = ac.Calculate(name, new double[] { 1, 2 });
            Assert.AreEqual(d[0], 4);

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B], [A]+[B]");
            d = ac.Calculate(name, new double[] { 3, 4 });
            CollectionAssert.AreEqual(d, new double[] { 7, 7 });

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, new TestAreaCalculator_Normal());
            d = ac.Calculate(name, new double[] { 2, 3 });
            CollectionAssert.AreEqual(d, new double[] { 3 });
        }

        [TestMethod]
        public void Calculate_CheckArgsOrder()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[C],[A],[B]");
            var d = ac.Calculate(name, new double[] { 1, 2, 3 });
            CollectionAssert.AreEqual(d, new double[] { 3, 1, 2 });

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, new TestAreaCalculator_CheckOrder());
            d = ac.Calculate(name, new double[] { 1, 2, 3 });
            CollectionAssert.AreEqual(d, new double[] { 3, 1, 2 });
        }

        [TestMethod]
        public void Calculate_CircleByRNormal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            double r = 1;
            var d = ac.Calculate("CircleByR", new double[] { r });

            Assert.AreEqual(d[0], Math.PI * r * r);
        }

        [TestMethod]
        public void Calculate_TriangleBySidesNormal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            double a = 3, b = 4, c = 5, s = 6;
            var d = ac.Calculate("TriangleBySides", new double[] { a, b, c });
            var v = ac.Calculate("TriangleBySides", new double[] { a + 1, b + 1, c + 1 });

            Assert.AreEqual(d[0], s);
            Assert.AreEqual(d[1], 1.0);
            Assert.AreEqual(v[1], 0.0);
        }

        [TestMethod]
        public void Calculate_QuadByABNormal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            double a = 2, b = 3;
            var d = ac.Calculate("QuadByAB", new double[] { a, b });

            CollectionAssert.AreEqual(d, new double[] { a * b });
        }

        [TestMethod]
        public void Calculate_ArgumentNullException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]");
            var vc = ac.GetFormulaVariables(name).Count;
            var d = GenerateArgs(vc);

            Assert.ThrowsException<ArgumentNullException>(
                () => ac.Calculate(name, null));
            Assert.ThrowsException<ArgumentNullException>(
                () => ac.Calculate(null, d));
        }

        [TestMethod]
        public void Calculate_ArgumentException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]");

            var vc = ac.GetFormulaVariables(name).Count;
            var d = GenerateArgs(vc + 1);

            Assert.ThrowsException<ArgumentException>(
                () => ac.Calculate(name, d));

            d = GenerateArgs(Math.Max(0, vc - 1));

            Assert.ThrowsException<ArgumentException>(
                () => ac.Calculate(name, d));
        }

        [TestMethod]
        public void Calculate_KeyNotFoundException()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);

            Assert.ThrowsException<KeyNotFoundException>(
                () => ac.Calculate(name, new double[1]));
        }

        [TestMethod]
        public void GetFormulaVariables_Normal()
        {
            var ac = new AreaCalculator.AreaCalculator();

            var name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, "[A]+[B]");

            CollectionAssert.AreEqual(ac.GetFormulaVariables(name),
                new List<string>() { "[A]", "[B]" });

            name = GenerateUniqueString(ac.FormulaNames);
            ac.AddFormula(name, new TestAreaCalculator_Normal());
            CollectionAssert.AreEqual(ac.GetFormulaVariables(name),
                 new List<string>() { "a", "h" });
        }

        private string GenerateUniqueString(ICollection<string> collection)
        {
            string n = "";
            var t = true;
            var rand = new Random();

            while (t)
            {
                n = rand.Next().ToString();
                if (collection is null) break;
                if (!collection.Contains(n)) break;
            }

            return n;
        }
        private double[] GenerateArgs(int count)
        {
            double[] a = new double[count];
            for (int i = 0; i < count; i++)
            {
                a[i] = i + 1;
            }
            return a;
        }
    }
}
