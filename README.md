## How to start

1. Create **AreaCalculator** class instance
2. Add custom formula by `AddFormula(string formulaName, string formula)` if needed
3. Run `Calculate(double[] args)` method

	AreaCalculator.AreaCalculator ac = new AreaCalculator.AreaCalculator();  
	ac.AddFormula("CircleByD", "{PI}*POW([d]/2,2)");  
	double[] r = ac.Calculate("CircleByD", new double[] { 10 });  


## Available builtin formulas

1. **CircleByR**. Calculates circle area by radius. Requires double[1] as input, where 0 element is radius. Returns double[1], where 0 element is area;
2. **TriangleBySides**. Calculates triangle area by sides length. Requires double[3] as input, where 0,1,2 elements is triangle sides length. Returns double[2], where 0 element is area and 1 element is check for right triangle (0 is triangle, 1 is right triangle).


## Add custom formula

You may add custom formula by `AddFormula(string formulaName, string formula)` method.  
May use:  

1. Math functions, can be obtained by `MathFuncs`< property. f.e.: SQRT()  
2. Constants, in curly brackets, can be obtained by `Constants` property. f.e.: {PI}  
3. Variables - one letter, in square brackets. f.e.: [x]  

Also, you can combine several formulas into one by separating them with a comma. f.e. [A]+[B], [A]+[C] returns [A+B, A+C]  
The results of the calculation will be returned in the corresponding elements of the output array.  

## Available  builtin constants

1. **{PI}** - Pi constant

## Available builtin math functions

1. **SQRT()** - square root function. Using: SQRT[A] = square root of [A]
2. **POW()** - power function. Using: POW[A, B] = a^b
3. **SIN()** - sinus function. Using: SIN([A]) = sinus of [A], [A] in radians.
4. **COS()** - cosinus function. Using: SIN([A]) = cosinus of [A], [A] in radians.

Also available:   
`+, -, *, /`  
`&&, ||, !` 
`>, <, >=, <=`  
`==, !=`  
`<condition>?<iftrue>:<iffalse>`  
