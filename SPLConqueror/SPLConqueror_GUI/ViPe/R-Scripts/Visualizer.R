meanNormalization <- function(dataToNormalize) {
  result <- NULL;
  
  minimumValue <- min(dataToNormalize);
  maximumValue <- max(dataToNormalize);
  meanValue <- mean(apply(dataToNormalize, 1, mean));
  
  result <- (dataToNormalize - meanValue) / (maximumValue - minimumValue);
  
  return(result);
}

visualize <- function(pathToExampleFiles, pathOfSourceFiles, pathToLibrary, doMeanNormalization=FALSE) {
  
  library("ggplot2", lib.loc=pathToLibrary)
  library("labeling", lib.loc=pathToLibrary)
  library("digest", lib.loc=pathToLibrary)
  #library(ggradar)
  
  suppressPackageStartupMessages(library("dplyr", lib.loc=pathToLibrary))
  library("scales", lib.loc=pathToLibrary)
  
  # The path to the working directory
  #path <- "C:/Users/chris_000/Desktop/Uni/ViPe/Examples/"
  setwd(pathToExampleFiles);

  performanceModels <- NULL;
  
  # Find all csv files in the current directory
  csvFiles <- list.files(".", pattern="\\.csv$", recursive = F);
  
  # Parse the performance models
  for (i in 1:length(csvFiles)) {
    file <- csvFiles[i];
    name <- strsplit(file, "\\.")[[1]][1];
    
    # Add the performance models
    performanceModel <- read.csv(file, header=TRUE, sep=";", check.names = FALSE);
    
    # Replace the feature interactions by numbers
    #performanceModel <- cbind(rep(name, nrow(performanceModel)), performanceModel);
    
    # Adjust the name of the column
    colnames(performanceModel)[1] <- "Group"
    if (i == 1) {
      performanceModels <- performanceModel;
    } else {
      performanceModels <- rbind(performanceModels, performanceModel)
    }
  }
  
  if (doMeanNormalization) {
    performanceModels[-1] <- meanNormalization(performanceModels[-1]);
  } else {
    # Find the maximum and minimum value
    maximumValue <- max(max(performanceModels[-1]), abs(min(performanceModels[-1])))
    minimumValue <- -maximumValue;
    
    performanceModels[-1] <- performanceModels[-1]  / maximumValue
  }
  
  source(paste(pathOfSourceFiles, "ggtext.R", sep=""))
  ggtext(performanceModels, text.font = "sans", text.size=14, pathOfSourceFiles = pathOfSourceFiles, pathToLibrary=pathToLibrary)
  source(paste(pathOfSourceFiles, "ggradar.R", sep=""))
  p <- ggradar(performanceModels, axis.label.size=3, grid.label.size=7, legend.text.size=14, font.radar = "sans", values.radar = c("", "", ""), grid.min = -1, grid.mid = 0, grid.max = 1, pathOfSourceFiles = pathOfSourceFiles, pathToLibrary = pathToLibrary)
  ggsave("StarPlot_1.pdf", height=8.5, width=11, p)
}