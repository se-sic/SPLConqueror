# SPLConqueror
SPL Conqueror is a library to learn the influence of configuration options of configurable software systems on non-functional properties.

The core project can be compiled without any further dependencies. It contains basic functionality to model a variable software system including configuration options and their constraints.

The machine-learning project defines different algorithms to learn the influence of configuration options on non-functional properties, such as performance or energy. It also specifies interfaces for satisfiability checking of configurations with respect to the variability model and optimization with respect to finding an optimal configuration for a given objective function and non-functional property. The implementation of these algorithms and methods are located in other assemblies / projects to ensure modularity, extensibility, and GPL support. We provide an assembly that relies on the Microsoft Solver Foundation's constraint-satisfaction-problem solver to implement SAT checking and some optimization algorithms.

The project SolverFoundationWrapper implements SAT checking and CSP tasks (optimization, linear programming) based on the Microsoft Solver Foundation. The project is linked to the SolverFoundation.dll, which is not part of the repository. To compile and work with this project, you need to download the SolverFoundation library from Microsoft: https://msdn.microsoft.com/en-us/devlabs/hh145003 . The library is free for academic use under the licence: "Microsoft Solver Foundation Academic Edition" (see for more information: https://msdn.microsoft.com/en-us/library/ff524499(v=vs.93).aspx). Note that we use version: v2.0.50727. Version 3.x is currently not supported.

