@echo off
set argC=0
for %%x in (%*) do Set /A argC+=1
echo %argc%
if %argc% LSS 3 (
	CALL :printUsage
	set /p temp= "Press enter to continue..."
	exit 0
)

set pathToCsvFiles=%1

set pathToLibDir="%2"

set pathToRscript="%3"

set currentDirectory=%cd%

echo Current directory: %currentDirectory%

if "%argc%" EQU "4" (
	:: Perform installation
	echo %pathToRscript% %currentDirectory%\InstallationWrapper.R %pathToLibDir% %currentDirectory%
	"%pathToRscript%" %currentDirectory%\InstallationWrapper.R "%pathToLibDir%" "%currentDirectory%"
)

if %errorlevel% GTR 0 (
	echo [Error] R-script execution failed while installing!
	set /p temp= "Press enter to continue...
	exit 1
)

:: Execute the R-scripts and pass arguments to it
echo %pathToRscript% %currentDirectory%\VisualizationWrapper.R "%pathToCsvFiles%" "%currentDirectory%" "%pathToLibDir%"
"%pathToRscript%" %currentDirectory%\VisualizationWrapper.R "%pathToCsvFiles%" "%currentDirectory%" "%pathToLibDir%"

if %errorlevel% GTR 0 (
	echo [Error] R-script execution failed!
	set /p temp="Press enter to continue..."
	exit 1
)

:: Copy the images for the legend
mkdir %pathToCsvFiles%\Resources
copy %currentDirectory%\Resources\*.png %pathToCsvFiles%\Resources\

:: Now, invoke PDFLaTeX
cd %pathToCsvFiles%
pdflatex StarPlot.tex
pdfcrop StarPlot.pdf StarPlot.pdf
pdflatex TextPlot.tex	
pdfcrop TextPlot.pdf TextPlot.pdf

:: Remove temporary files that are no longer needed
::rm StarPlot_1.pdf
::rm TextPlot_1.pdf
DEL Rplots.pdf
DEL *.aux 
DEL *.log

:: print usage function
:printUsage
echo "Usage: ./Visualizer.sh <PathToDirectoryWithCsvFiles> <PathToLibDir> [PathToRscript] [PerformInstallation]";
echo "PathToDirectoryWithCsvFiles is the path to the directory containing the performance models as .csv-files.";
echo "PathToLibDir is the path where the libraries should be stored.";
echo "PathToRscript is the path to the Rscript.exe file (only needed on Windows) - Default: Rscript";
echo "PerformInstallation tells the script whether the installation of the libraries should be performed or not.";