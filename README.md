# SPLConqueror
SPL Conqueror is a library to learn the influence of configuration options of configurable software systems on non-functional properties.

The core project can be compiled without any further dependencies. It contains basic functionality to model a variable software system including configuration options and their constraints.

The machine-learning project defines different algorithms to learn the influence of configuration options on non-functional properties, such as performance or energy. It also specifies interfaces for satisfiability checking of configurations with respect to the variability model and optimization with respect to finding an optimal configuration for a given objective function and non-functional property. The implementation of these algorithms and methods are located in other assemblies / projects to ensure modularity, extensibility, and GPL support. We provide an assembly that relies on the Microsoft Solver Foundation's constraint-satisfaction-problem solver to implement SAT checking and some optimization algorithms.

The project SolverFoundationWrapper implements SAT checking and CSP tasks (optimization, linear programming) based on the Microsoft Solver Foundation. The project is linked to the SolverFoundation.dll. You have to install the [redestribution package]{/SPLConqueror/SolverFoundationWrapper/en_solver_foundation_academic_edition_redistributable_installation_v3.1_x64_742237.msi} first so that the wrapper can find the required dll.

## How to install SPLConqueror

###### On a Mac (OS X (10.11.6))
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

## How to use SPLConqueror

### GUI

SPL Conqueror provides four different graphical user interfaces.

1. VariabilityModel_GUI

The VariabilityModel_GUI can be used to define the variability model of a configurable system or to modify existing models. To create a new variability model for a system, fist use *File>New Model*. Afterwards, an empty model containing only of the root feature is created. New features can be added to the model by a right click on a feature. In the *Create new Feature* dialogue, it is possible to define whether the new feature is a binary or a numeric one. For numeric features, also a minimal and maximal value of the value domain have to defined. Besides, if only a subset of all values between the minimal and the maximal value of the domain are allowed, a specific step function can be defined. In this function it is possible to use an alias for the numeric option (*n*). In the following, we give two examples of the step functions: 

  * n + 2 (using this function, only even or odd values depending on the minimal value are allowed)
  * n * 2 (using this function, the minimal value is multiplied by two until the maximal value is reached)

Additionally, constraints between different configuration options can be defined using *Edit>Edit Constraints*. Last, an alternative group of options can be created using *Edit>Edit Alternative Groups*.

2. PerformancePrediction_GUI

The PerformancePrediction_Gui provides an interface to learn performance-influence models. 
To use this GUI, first a variability model and dedicated measurements of the system has to be defined.
Afterwards, in the middle are of the GUI, a set of binary and numeric sampling strategies has to be selected to define a set of configuration used in the learning process. 
Last, to customize the machine-learning algorithm a set of parameters can be defined. 
Please make sure that bagging will be set to false when using this GUI. 

After the learning is started, the models, which are learned in an iterative manner are displayed in the lower part of the GUI. 
Here, the model is split by the different terms, where each term described the identified influence of an individual option or an interaction between options. 

3. SPLConqueror_GUI

This GUI can be used to visualize a learned performance-influence model. 

4. Script generator

The Script generator can be used to defined .a script files that are needed in the CommandLine project. 

