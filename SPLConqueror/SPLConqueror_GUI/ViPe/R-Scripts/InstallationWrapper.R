# Read in the arguments
# The first argument is the path to the csv-files
# The second argument is the path to the source files
# The third argument is the path to the libraries
args <- commandArgs(TRUE);
pathToLibraries <- args[1];
pathToSourceFiles <- args[2];
pathToLibrary <- args[3];

if (!endsWith(pathToLibraries, "/")) {
  pathToLibraries <- paste(pathToLibraries, "/", sep="");
}

if (!endsWith(pathToSourceFiles, "/")) {
  pathToSourceFiles <- paste(pathToSourceFiles, "/", sep="");
}

source(paste(pathToSourceFiles, "Installation.R", sep=""));
installNeededPackages(pathToLibraries)