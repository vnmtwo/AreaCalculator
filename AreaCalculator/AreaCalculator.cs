using System.Collections.Generic;

namespace AreaCalculator
{
    public class AreaCalculator
    {
        private Dictionary<string, string> Constants;
        private Dictionary<string, string> MathFunc;
        private Dictionary<string, Formula> Formulas;

        public AreaCalculator()
        {
            Constants = new Dictionary<string, string>();
            MathFunc = new Dictionary<string, string>();
            Formulas = new Dictionary<string, Formula>();
            
            Constants.Add("{PI}", "Math.PI");
            
            MathFunc.Add("SQRT", "Math.Sqrt");
            MathFunc.Add("POW", "Math.Pow");
            MathFunc.Add("SIN", "Math.Sin");
            MathFunc.Add("COS", "Math.Cos");

            AddFormula("CircleByR", "{PI}*POW([r],2)");
            AddFormula("TriangleBySides",
                @"SQRT(
                    (([a]+[b]+[c])/2)*
                    (([a]+[b]+[c])/2-[a])*
                    (([a]+[b]+[c])/2-[b])*
                    (([a]+[b]+[c])/2-[c])
                ),
                (([a]*[a]==[b]*[b]+[c]*[c])||
                ([b]*[b]==[a]*[a]+[c]*[c])||
                ([c]*[c]==[a]*[a]+[b]*[b]))?1:0");
        }

        /// <summary>
        /// Calculates the given formula.
        /// </summary>
        /// <param name="formulaName">
        /// Formula name from Formulas list. 
        /// The list can be obtained by calling <c>GetFormulasNames</c>.
        /// </param>
        /// <param name="args">
        /// Variables values, placed alphabetically in array.
        /// For example: formula is <c>[b]+[c]+[a]</c>
        /// values is b=2 a=1 c=3
        /// args array must be formed: <c>[1,2,3]</c>
        /// </param>
        /// <returns>
        /// Array of results corresponding to the formula.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">If formulaName is null</exception>
        /// <exception cref="KeyNotFoundException">If formulaName is not exists in formulas list</exception>
        public double[] Calculate(string formulaName, double[] args)
        {
           return Formulas[formulaName].Calculate(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>All formulas names, available for execution.</returns>
        public List<string> GetFormulasNames()
        {
            return new List<string>(Formulas.Keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// All math functions names, available for use.
        /// Also available: 
        /// +, -, *, /
        /// &&, ||, !
        /// >, <, >=, <=
        /// ==, !=
        /// <condition>?<iftrue>:<iffalse>
        /// </returns>
        public List<string> GetMathFunctionsNames()
        {
            return new List<string>(MathFunc.Keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// All constant names, available for use.
        /// </returns>
        public List<string> GetConstantsNames()
        {
            return new List<string>(Constants.Keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formulaName"></param>
        /// <returns>
        /// List of variables, used in formula.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">If formulaName is null</exception>
        /// <exception cref="KeyNotFoundException">If formulaName is not exists in formulas list</exception>
        public List<string> GetFormulaVariables(string formulaName)
        {
            return Formulas[formulaName].GetVariables();
        }

        /// <summary>
        /// Add and compile new formula. formulaName must be unique.
        /// </summary>
        /// <param name="formulaName">
        /// Unique value to store formula. Must be unique.
        /// </param>
        /// <param name="formula">
        /// Formula for calculation.
        /// May use:
        /// 1) Math functions, see <c>GetFormulasNames()</c>. f.e.: SQRT()
        /// 2) Constants, in curly brackets, see <c>GetConstantsNames()</c>. f.e.: {PI}
        /// 3) Variables - one letter, in square brackets. f.e.: [x]
        /// </param>
        /// <exception cref="System.ArgumentNullException">If formulaName is null.</exception>
        /// <exception cref="System.ArgumentException">If formulaName is already used.</exception>
        /// <exception cref="FormulaCompilationException">If formula is incorrect.</exception>
        /// <exception cref="FormulaWithoutVariablesException">If variables not found in formula.</exception>
        public void AddFormula(string formulaName, string formula)
        {
            foreach (var kp in Constants)
            {
                formula = formula.Replace(kp.Key, kp.Value);
            }
            foreach (var kp in MathFunc)
            {
                formula = formula.Replace(kp.Key, kp.Value);
            }
            
            Formulas.Add(formulaName, new Formula(formula));
        }

    }
}