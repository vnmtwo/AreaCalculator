using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AreaCalculator
{
    internal class Formula
    {
        private List<string> VariablesNames;
        private ICalculateArea Calculator;
        private string FormulaStr; //stored for next version features

        internal Formula(string formula)
        {
            FormulaStr = formula;
            VariablesNames = GetVariables(formula);

            Assembly assembly = null;
            CSharpCodeProvider cs_code_provider = new CSharpCodeProvider();
#pragma warning disable CS0618 // Тип или член устарел
            ICodeCompiler cs_code_compiler = cs_code_provider.CreateCompiler();
#pragma warning restore CS0618 // Тип или член устарел
            CompilerParameters compiler_parameters = new CompilerParameters();

            compiler_parameters.ReferencedAssemblies.Add("System.dll");
            compiler_parameters.ReferencedAssemblies.Add("System.Data.dll");
            compiler_parameters.ReferencedAssemblies.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\AreaCalculator.dll");

            compiler_parameters.GenerateInMemory = true;
            compiler_parameters.GenerateExecutable = false;
            compiler_parameters.CompilerOptions = "/optimize";

            string class_str =
                @"
                    using System;
                    namespace AreaCalculator
                    {
                        internal class AreaCalculatorFormula : ICalculateArea
                        {
                            public double[] CalculateArea(double[] args)
                            {
                                %variables%

                                return new double[]
                                {
                                    %formula%
                                };
                            }
                        }
                    }
                    ";

            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (var variable in VariablesNames)
            {
                sb.Append("double " + CleanBrackets(variable) + " = args[" + index++.ToString() + "];");
            }

            class_str = class_str.Replace("%variables%", sb.ToString());
            class_str = class_str.Replace("%formula%", formula.Replace("[", "").Replace("]", ""));

            CompilerResults compiler_results = cs_code_compiler.CompileAssemblyFromSource(compiler_parameters, class_str);
            if (compiler_results.Errors.Count > 0)
                throw new FormulaCompilationException(compiler_results.Errors[0].ToString());

            assembly = compiler_results.CompiledAssembly;

            foreach (Type t in assembly.GetTypes())
            {
                if (typeof(ICalculateArea).IsAssignableFrom(t))
                {
                    object o = Activator.CreateInstance(t);
                    Calculator = o as ICalculateArea;
                    break;
                }
            }
        }

        public Formula(ICalculateArea calculator)
        {
            FormulaStr = null;
            Calculator = calculator;
            VariablesNames = new List<string>();

            var fields_array = calculator.GetType().GetFields();
            foreach (var f in fields_array)
            {
                if (f.GetCustomAttribute(typeof(FormulaFieldAttribute), false) != null)
                {
                    VariablesNames.Add(f.Name);
                }
            }

            VariablesNames.Sort();

            if (VariablesNames.Count == 0)
                throw new ClassNoContainsFormulaFieldsException();
        }

        internal double[] Calculate(double[] args)
        {
            if (args is null)
                throw new ArgumentNullException("args");
            if (VariablesNames.Count != args.Length)
                throw new ArgumentException(
                    "Input arguments count does not match the number of variables in the formula");

            return Calculator.CalculateArea(args);
        }
        private List<string> GetVariables(string formula)
        {
            Regex regex = new Regex(@"[\[][\S][\]]");
            MatchCollection matches = regex.Matches(formula);
            if (matches.Count == 0) throw new FormulaWithoutVariablesException();

            List<string> variables = new List<string>();
            foreach (Match match in matches)
            {
                if (!variables.Contains(match.Value))
                    variables.Add(match.Value);
            }
            variables.Sort();
            return variables;
        }
        internal List<string> GetVariables()
        {
            return VariablesNames;
        }
        private string CleanBrackets(string s)
        {
            return s.Substring(1, s.Length - 2);
        }
    }
}