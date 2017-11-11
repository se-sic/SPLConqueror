# Install ggplot2
installNeededPackages <- function(path) {
  #.libPaths(c(path, .libPaths()));

  install.packages("ggplot2", lib=path, repos="https://cran.uni-muenster.de/")
  install.packages("gridExtra", lib=path, repos="https://cran.uni-muenster.de/")
  install.packages("scales", lib=path, repos="https://cran.uni-muenster.de/")

  install.packages("cowplot", lib=path, repos="https://cran.uni-muenster.de/")

  # Install devtools for downloading the newest version of some packages
  install.packages("devtools", lib=path, repos="https://cran.uni-muenster.de/")

  # Install ggradar for the radar-plots
  #devtools::install_github("ricardo-bion/ggradar",
  #                         dependencies=TRUE)

  # Install tibble (as a dependency for dplyr)
  install.packages("tibble", lib=path, repos="https://cran.uni-muenster.de/")
  
  install.packages("dplyr", lib=path, repos="https://cran.uni-muenster.de/")
  # library("withr", lib.loc=path)
  # library("httr", lib.loc=path)
  # library("curl", lib.loc=path)
  # library("devtools", lib.loc=path)
  # # Install dplyr by using devtools (this could take a while)
  # withr::with_libpaths(new = path,devtools::install_github("tidyverse/dplyr"))
}