## How to start

1. Create **AreaCalculator** class instance
2. Add your formula by `AddFormula(string formulaName, string formula)` if needed
3. Run `Calculate(double[] args)` method

	AreaCalculator.AreaCalculator ac = new AreaCalculator.AreaCalculator();  
	ac.AddFormula("CircleByD", "{PI}*POW([d]/2,2)");  
	double[] r = ac.Calculate("CircleByD", new double[] { 10 });  


## Available builtin formulas

1. **CircleByR**. Calculates circle area by radius. Requires double[1] as input, where 0 element is radius. Returns double[1], where 0 element is area;
2. **TriangleBySides**. Calculates triangle area by sides length. Requires double[3] as input, where 0,1,2 elements is triangle sides length. Returns double[2], where 0 element is area and 1 element is check for right triangle (0 is triangle, 1 is right triangle).


	