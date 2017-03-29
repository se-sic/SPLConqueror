# SPLConqueror
SPL Conqueror is a library to learn the influence of configuration options of configurable software systems on non-functional properties.

The core project can be compiled without any further dependencies. It contains basic functionality to model a variable software system including configuration options and their constraints.

The machine-learning project defines different algorithms to learn the influence of configuration options on non-functional properties, such as performance or energy. It also specifies interfaces for satisfiability checking of configurations with respect to the variability model and optimization with respect to finding an optimal configuration for a given objective function and non-functional property. The implementation of these algorithms and methods are located in other assemblies / projects to ensure modularity, extensibility, and GPL support. We provide an assembly that relies on the Microsoft Solver Foundation's constraint-satisfaction-problem solver to implement SAT checking and some optimization algorithms.

The project SolverFoundationWrapper implements SAT checking and CSP tasks (optimization, linear programming) based on the Microsoft Solver Foundation. The project is linked to the SolverFoundation.dll. You have to install the [redestribution package]{/SPLConqueror/SolverFoundationWrapper/en_solver_foundation_academic_edition_redistributable_installation_v3.1_x64_742237.msi} first so that the wrapper can find the required dll.

## How to install SPLConqueror

######On a Mac (OS X (10.11.6))
1. Clone git repository

2. Download and install latest Xamarin-IDE from https://www.xamarin.com

3. mkdir "<SPLConquerer-GitRoot>/SPLConqueror/dll"

4. Copy "Microsoft.Solver.Foundation.dll" (>= v3.0.0) to "<SPLConquerer-GitRoot>/SPLConqueror/dll"

5. mkdir "<SPLConquerer-GitRoot>/SPLConqueror/packages"

6. cd "<SPLConquerer-GitRoot>/SPLConqueror/packages"

7. Install dependencies:

  * nuget install Accord -version 2.12.0.0
  * nuget install Accord.Math -version 2.12.0.0
  * nuget install AForge -version 2.2.5
  * nuget install AForge.Math -version 2.2.5
  * nuget install ILNumerics -version 3.3.3.0


8. Open root project "<SPLConquerer-GitRoot>/SPLConquerorSPLConqueror.sln" in Xamarin

9. In subproject "SolverFoundationWrapper" double click on references and point entry "Microsoft.Solver.Foundation.dll" to "<SPLConquerer-GitRoot>/SPLConqueror/dll/Microsoft.Solver.Foundation.dll"

10. Build root project