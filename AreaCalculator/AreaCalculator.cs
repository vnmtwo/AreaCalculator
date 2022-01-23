using System.Collections.Generic;
using System;

namespace AreaCalculator
{
    public class AreaCalculator
    {
        private Dictionary<string, string> _constants;
        private Dictionary<string, string> _mathFuncs;

        private Dictionary<string, Formula> _formulas;

        public List<string> Constants
        {
            get
            {
                return GetConstantsNames();
            }
        }

        public List<string> MathFuncs
        {
            get
            {
                return GetMathFunctionsNames();
            }
        }

        public List<string> FormulaNames
        {
            get
            {
                return GetFormulasNames();
            }
        }

        public AreaCalculator()
        {
            _constants = new Dictionary<string, string>();
            _mathFuncs = new Dictionary<string, string>();
            _formulas = new Dictionary<string, Formula>();
            
            _constants.Add("{PI}", "Math.PI");

            _mathFuncs.Add("SQRT", "Math.Sqrt");
            _mathFuncs.Add("POW", "Math.Pow");
            _mathFuncs.Add("SIN", "Math.Sin");
            _mathFuncs.Add("COS", "Math.Cos");

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
            foreach (var kp in _constants)
            {
                formula = formula.Replace(kp.Key, kp.Value);
            }
            foreach (var kp in _mathFuncs)
            {
                formula = formula.Replace(kp.Key, kp.Value);
            }

            _formulas.Add(formulaName, new Formula(formula));
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
        /// <exception cref="ArgumentException">If args length doesn't match variables count in formula</exception>
        /// <exception cref="ArgumentNullException">If formulaName is null or arg is null</exception>
        /// <exception cref="KeyNotFoundException">If formulaName is not exists in formulas list</exception>
        public double[] Calculate(string formulaName, double[] args)
        {
           return _formulas[formulaName].Calculate(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>All formulas names, available for execution.</returns>
        private List<string> GetFormulasNames()
        {
            return new List<string>(_formulas.Keys);
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
        private List<string> GetMathFunctionsNames()
        {
            return new List<string>(_mathFuncs.Keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// All constant names, available for use.
        /// </returns>
        private List<string> GetConstantsNames()
        {
            return new List<string>(_constants.Keys);
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
            return _formulas[formulaName].GetVariables();
        }



    }
}