using System.Collections.Generic;
using System;

namespace AreaCalculator
{
    /// <summary>
    /// Represents area calculator class.
    /// </summary>
    public class AreaCalculator
    {
        private Dictionary<string, string> _constants;
        private Dictionary<string, string> _mathFuncs;
        private Dictionary<string, Formula> _formulas;

        /// <summary>
        /// All constant names, available for use.
        /// </summary>
        public List<string> Constants
        {
            get
            {
                return GetConstantsNames();
            }
        }
        /// <summary>
        /// All math functions names, available for use.
        /// Also available: 
        /// +, -, *, /
        /// &&, ||, !
        /// >, <, >=, <=
        /// ==, !=
        /// <condition>?<iftrue>:<iffalse>
        /// </summary>
        public List<string> MathFuncs
        {
            get
            {
                return GetMathFunctionsNames();
            }
        }
        /// <summary>
        /// All formulas names, available for execution.
        /// </summary>
        public List<string> FormulaNames
        {
            get
            {
                return GetFormulasNames();
            }
        }

        /// <summary>
        /// Initializes new instance of AreaCalculator class.
        /// </summary>
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
        /// Unique value to store formula.
        /// </param>
        /// <param name="formula">
        /// Formula for calculation.
        /// May use:
        /// 1) Math functions, can be obtained by <c>MathFuncs</c> property. f.e.: SQRT()
        /// 2) Constants, in curly brackets, can be obtained by <c>Constants</c> property. f.e.: {PI}
        /// 3) Variables - one letter, in square brackets. f.e.: [x]
        /// Also, you can combine several formulas into one by separating them with a comma.
        /// The results of the calculation will be returned in the corresponding elements of the output array.
        /// </param>
        /// <exception cref="ArgumentNullException">If formulaName is null or formula is null</exception>
        /// <exception cref="ArgumentException">If formulaName is already used.</exception>
        /// <exception cref="FormulaCompilationException">If formula is incorrect.</exception>
        /// <exception cref="FormulaWithoutVariablesException">If variables not found in formula.</exception>
        public void AddFormula(string formulaName, string formula)
        {
            if (formula == null)
                throw new ArgumentNullException("formula is null");

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
        /// 
        /// </summary>
        /// <param name="formulaName">
        /// Formula name from Formulas list. 
        /// The list can be obtained by <c>FormulaNames</c> property.
        /// </param>
        /// <returns>
        /// List of variables, used in formula.
        /// </returns>
        /// <exception cref="ArgumentNullException">If formulaName is null</exception>
        /// <exception cref="KeyNotFoundException">If formulaName is not exists in formulas list</exception>
        public List<string> GetFormulaVariables(string formulaName)
        {
            return _formulas[formulaName].GetVariables();
        }

        /// <summary>
        /// Calculates the given formula.
        /// </summary>
        /// <param name="formulaName">
        /// Formula name from Formulas list. 
        /// The list can be obtained by <c>FormulaNames</c> property.
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
        /// <exception cref="ArgumentNullException">If formulaName is null or args is null</exception>
        /// <exception cref="KeyNotFoundException">If formulaName is not exists in formulas list</exception>
        public double[] Calculate(string formulaName, double[] args)
        {
           return _formulas[formulaName].Calculate(args);
        }

        private List<string> GetFormulasNames()
        {
            return new List<string>(_formulas.Keys);
        }

        private List<string> GetMathFunctionsNames()
        {
            return new List<string>(_mathFuncs.Keys);
        }

        private List<string> GetConstantsNames()
        {
            return new List<string>(_constants.Keys);
        }
    }
}