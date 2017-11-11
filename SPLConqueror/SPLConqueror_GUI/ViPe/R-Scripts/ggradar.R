#' ggradar
#' @author Ricardo Bion
#' @export ggradar
#'
#' @export
# most of the code is from http://rstudio-pubs-static.s3.amazonaws.com/5795_e6e6411731bb4f1b9cc7eb49499c2082.html


ggradar <- function(plot.data,
                             font.radar="Circular Air Light",
                             values.radar = c("0%", "50%", "100%"),                       
                             axis.labels=colnames(plot.data)[-1],                             
                             grid.min=0,  #10,
                             grid.mid=0.5,  #50,
                             grid.max=1,  #100,
                             centre.y=grid.min - ((1/9)*(grid.max-grid.min)),
                             plot.extent.x.sf=1,
                             plot.extent.y.sf=1.2,
                             x.centre.range=0.02*(grid.max-centre.y),
                             label.centre.y=FALSE,
                             grid.line.width=0.5,
                             gridline.min.linetype="longdash",
                             gridline.mid.linetype="longdash",
                             gridline.max.linetype="longdash",
                             gridline.min.colour="grey",
                             gridline.mid.colour="#007A87",
                             gridline.max.colour="grey",
                             grid.label.size=7,
                             gridline.label.offset=-0.1*(grid.max-centre.y),
                             label.gridline.min=TRUE,
                             axis.label.offset=1.15,
                             axis.label.size=8,
                             axis.line.colour="grey",
                             group.line.width=1.5,
                             group.point.size=6,
                             background.circle.colour="#D7D6D1",
                             background.circle.inner.colour = "indianred1", 
                             background.circle.outer.colour = "green",
                             background.circle.transparency=0.2,
                             plot.legend=if (nrow(plot.data)>1) TRUE else FALSE,
                             legend.title="",
                             plot.title="",
                             legend.text.size=9,
                             line.offset = 0.5,
                             pathOfSourceFiles,
                             pathToLibrary) {

  library("ggplot2", lib.loc=pathToLibrary)
  # Retrieve the location of the script
  #script.dir <- dirname(sys.frame(1)$ofile)
  # Load the script containing commonly used functions
  source(paste(pathOfSourceFiles, "common.R", sep=""))

  plot.data <- as.data.frame(plot.data)

  plot.data[,1] <- as.factor(as.character(plot.data[,1]))
  names(plot.data)[1] <- "group"

  var.names <- colnames(plot.data)[-1]  #'Short version of variable names 
  #axis.labels [if supplied] is designed to hold 'long version' of variable names
  #with line-breaks indicated using \n

  #caclulate total plot extent as radius of outer circle x a user-specifiable scaling factor
  plot.extent.x=(grid.max+abs(centre.y))*plot.extent.x.sf
  plot.extent.y=(grid.max+abs(centre.y))*plot.extent.y.sf

  #Check supplied data makes sense
  if (length(axis.labels) != ncol(plot.data)-1) 
    return("Error: 'axis.labels' contains the wrong number of axis labels") 
  if(min(plot.data[,-1])<centre.y)
    return("Error: plot.data' contains value(s) < centre.y")
  if(max(plot.data[,-1])>grid.max)
    return("Error: 'plot.data' contains value(s) > grid.max")
#Declare required internal functions

CalculateGroupPath <- function(df,rv) {
  #Converts variable values into a set of radial x-y coordinates
  #Code adapted from a solution posted by Tony M to
  #http://stackoverflow.com/questions/9614433/creating-radar-chart-a-k-a-star-plot-spider-plot-using-ggplot2-in-r
  #Args:
  #  df: Col 1 -  group ('unique' cluster / group ID of entity)
  #      Col 2-n:  v1.value to vn.value - values (e.g. group/cluser mean or median) of variables v1 to v.n
  path <- df[,1]

  ##find increment
  angles = seq(from=0, to=2*pi, by=(2*pi)/(ncol(df)-1))
  ##create graph data frame
  graphData= data.frame(seg="", x=0,y=0,val=0)
  graphData=graphData[-1,]
  #allLevels = levels(path);

  for(count in 1:length(path)){
    i = path[count];
    pathData = subset(df, df[,1]==i)
    for(j in c(2:ncol(df))){
      #pathData[,j]= pathData[,j]
      
      graphData=rbind(graphData, data.frame(group=i, 
                                            x=pathData[,j]*sin(angles[j-1]),
                                            y=pathData[,j]*cos(angles[j-1]), 
                                            val=rv[count,j]))
    }
    ##complete the path by repeating first pair of coords in the path
    graphData=rbind(graphData, data.frame(group=i, 
                                          x=pathData[,2]*sin(angles[1]),
                                          y=pathData[,2]*cos(angles[1]),
                                          val=rv[count,2]))
  }
  #Make sure that name of first column matches that of input data (in case !="group")
  colnames(graphData)[1] <- colnames(df)[1]
  graphData #data frame returned by function
}
CalculateAxisPath = function(var.names,min,max) {
  #Caculates x-y coordinates for a set of radial axes (one per variable being plotted in radar plot)
  #Args:
  #var.names - list of variables to be plotted on radar plot
  #min - MININUM value required for the plotted axes (same value will be applied to all axes)
  #max - MAXIMUM value required for the plotted axes (same value will be applied to all axes)
  #var.names <- c("v1","v2","v3","v4","v5")
  n.vars <- length(var.names) # number of vars (axes) required
  #Cacluate required number of angles (in radians)
  angles <- seq(from=0, to=2*pi, by=(2*pi)/n.vars)
  #calculate vectors of min and max x+y coords
  min.x <- min*sin(angles)
  min.y <- min*cos(angles)
  max.x <- max*sin(angles)
  max.y <- max*cos(angles)
  #Combine into a set of uniquely numbered paths (one per variable)
  axisData <- NULL
  for (i in 1:n.vars) {
    a <- c(i,min.x[i],min.y[i])
    b <- c(i,max.x[i],max.y[i])
    axisData <- rbind(axisData,a,b)
  }
  #Add column names + set row names = row no. to allow conversion into a data frame
  colnames(axisData) <- c("axis.no","x","y")
  rownames(axisData) <- seq(1:nrow(axisData))
  #Return calculated axis paths
  as.data.frame(axisData)
}
funcCircleCoords <- function(center = c(0,0), r = 1, npoints = 100){
  #Adapted from Joran's response to http://stackoverflow.com/questions/6862742/draw-a-circle-with-ggplot2
  tt <- seq(0,2*pi,length.out = npoints)
  xx <- center[1] + r * cos(tt)
  yy <- center[2] + r * sin(tt)
  return(data.frame(x = xx, y = yy))
}

SplitInHalf <- function(vector) {
  result <- list();
  vectorLength <- length(vector);
  tmp <- c();
  for (i in 1:round(vectorLength / 2)) {
    tmp <- c(tmp, vector[i]);
  }
  result <- c(result, list(tmp));
  tmp <- c();
  for (i in (round(vectorLength / 2) + 1) : vectorLength) {
    tmp <- c(tmp, vector[i]);
  }
  result <- c(result, list(tmp));
  
  return(result);
}

GenerateTexFile <- function(filePath, pathToOutputFile, allTerms, titles) {
  content <- c(
    "\\documentclass{standalone}",
    "",
    "\\usepackage{color}",
    "\\usepackage{graphicx}",
    "\\usepackage{tikz}",
    "\\usetikzlibrary{positioning, calc}",
    "",
    "",
    "\\begin{document}",
    "\t\\newcommand{\\picHeight}{400px}",
    "\t\\newcommand{\\picWidth}{520px}",
    "",
    "\t\\newcommand{\\PI}{3.141592}",
    "",
    "\t\\newcommand{\\centerX}{9.22}",
    "\t\\newcommand{\\centerY}{-7}",
    "\t\\newcommand{\\radius}{3.3}",
    "\t\\newcommand{\\lineOffset}{0.7}",
    "\t\\newcommand{\\textsize}{\\tiny}",
    "\t\\newcommand{\\offset}{0.2}",
    ""
  );
  
  # Create pgf macros for the coordinates
  numberLabels <- length(allTerms);
  
  content <- c(content, 
               c(
                 paste("\t\\newcommand{\\angleDiff}{360 / ", numberLabels, "}", sep ="")
               ))
  
  prefixes <- c(letters, LETTERS);
  
  for (i in 1:numberLabels) {
    xCoordinateLabel <- paste("\t\\pgfmathsetmacro\\", prefixes[i], "x{\\centerX + (\\radius + \\lineOffset) * sin(\\angleDiff * ", i - 1, ")", sep="");
    yCoordinateLabel <- paste("\t\\pgfmathsetmacro\\", prefixes[i], "y{\\centerY + (\\radius + \\lineOffset) * cos(\\angleDiff * ", i - 1, ")", sep="");
    
    
    if (i == 1) {
      yCoordinateLabel <- paste(yCoordinateLabel, " + \\offset", sep="")
    } else if ((i - 1) == numberLabels / 2) {
      yCoordinateLabel <- paste(yCoordinateLabel, " - \\offset", sep="")
    } else if ((i - 1) < numberLabels / 2) {
      xCoordinateLabel <- paste(xCoordinateLabel, " + \\offset", sep="")
    } else {
      xCoordinateLabel <- paste(xCoordinateLabel, " - \\offset", sep="")
    }

    xCoordinateLabel <- paste(xCoordinateLabel, "}", sep="")
    yCoordinateLabel <- paste(yCoordinateLabel, "}", sep="")
    
    content <- c(content, 
                 c(
                   xCoordinateLabel,
                   yCoordinateLabel
                 ))
  }
  
  content <- c(content, 
               c("",
                 "\t% The definitions for the legend",
                 "\t\\newcommand{\\legendOffset}{3}",
                 "\t\\newcommand{\\spaceBetweenLegendColumns}{0.8}",
                 "\t\\newcommand{\\spaceBetweenLegendRows}{0.6}",
                 "\t\\newcommand{\\outerMarginArea}{0.3}",
                 "\t\\newcommand{\\spaceBetweenLegendTitleAndLegend}{1}",
                 "",
                 "\t% Macros for the legend",
                 "\t\\pgfmathsetmacro\\legendX{\\centerX}",
                 "\t\\pgfmathsetmacro\\legendY{\\centerY - \\radius - \\legendOffset}",
                 "\t\\pgfmathsetmacro\\legendTitleX{\\legendX}",
                 "\t\\pgfmathsetmacro\\legendTitleY{\\legendY - \\outerMarginArea}",
                 "\t\t\\pgfmathsetmacro\\firstFillnessTitleX{\\legendX - \\spaceBetweenLegendColumns / 2}",
                 "\t\t\\pgfmathsetmacro\\firstFillnessTitleY{\\legendY - \\outerMarginArea - \\spaceBetweenLegendTitleAndLegend}",
                 "\t\t\\pgfmathsetmacro\\secondFillnessTitleX{\\legendX - \\spaceBetweenLegendColumns / 2}",
                 "\t\t\\pgfmathsetmacro\\secondFillnessTitleY{\\legendY - \\outerMarginArea - \\spaceBetweenLegendTitleAndLegend - \\spaceBetweenLegendRows}",
                 "\t\\pgfmathsetmacro\\firstColorPicX{\\legendX + \\spaceBetweenLegendColumns / 2}",
                 "\t\\pgfmathsetmacro\\firstColorPicY{\\legendY - \\outerMarginArea - \\spaceBetweenLegendTitleAndLegend}",
                 "\t\\pgfmathsetmacro\\secondColorPicX{\\legendX + \\spaceBetweenLegendColumns / 2}",
                 "\t\\pgfmathsetmacro\\secondColorPicY{\\legendY - \\outerMarginArea - \\spaceBetweenLegendTitleAndLegend - \\spaceBetweenLegendRows}",
                 ""
               ));
  
  
  content <- c(content,
               c(
                 "",
                 "\t\\begin{tikzpicture}",
                 paste("\t\t\\node[inner sep=0, anchor=north west] (pic) at (0,0) {\\includegraphics[width=\\picWidth, height=\\picHeight]{", pathToOutputFile,"}};", sep="")
               ));
  
  for (i in 1:numberLabels) {
    if (i == 1) {
      nodeProp <- "anchor=south"
    } else if ((i - 1) == numberLabels / 2) {
      nodeProp <- "anchor=north"
    } else if ((i - 1) < numberLabels / 2) {
      nodeProp <- "anchor=west, align=left"
    } else {
      nodeProp <- "anchor=east, align=right"
    }
    
    content <- c(content, 
                 c(
                   paste("\t\t\\node[inner sep=0, ", nodeProp, "] at (\\", prefixes[i], "x, \\", prefixes[i], "y) {$ ", allTerms[i], " $};", sep="")
                 ))
  }
  
  legendContent <- c("",
      "\t\t% The legend",
      "\t\t\\node[inner sep=0, anchor = north, align=center] at (\\legendTitleX, \\legendTitleY) {\\Large \\textbf{Legend}};",
      "",
      "\t\t% Include legend for the fillness",
      "\t\t\\node[inner sep=0, anchor = east, align=right] (firstLeftLegendText) at (\\firstFillnessTitleX, \\firstFillnessTitleY) {Relevant influence};",
      "\t\t\\node[inner sep=0, anchor=south east, left = 0.1 of firstLeftLegendText] (emptyCircle) {\\includegraphics[width=10px, height=10px]{Resources/FullCircle.png}};",
      "",
      "\t\t\\node[inner sep=0, anchor=north, below = 0.1 of emptyCircle] (fullCircle) {\\includegraphics[width=10px, height=10px]{Resources/EmptyCircle.png}};",
      "\t\t\\node[inner sep=0, anchor = west, align=left, right = 0.1 of fullCircle] (secondLeftLegendText) {No relevant influence};",
      "\t\t% Include legend for the colors",
      "\t\t\\node[inner sep=0, yshift=-4, anchor=south west, align=center] (firstColor) at (\\firstColorPicX, \\firstColorPicY) {\\includegraphics[width=25px, height=10px]{Resources/FirstColor.png}};",
      paste("\t\t\\node[inner sep=0, anchor=south west, align=center, right = 0.1 of firstColor] (firstColorLabel) {", titles[1], "};", sep="")
    )
  if (length(titles) == 2) {
    legendContent <- c(legendContent, 
                       "\t\t\\node[inner sep=0, yshift=-4, anchor=south west, align=center] (secondColor) at (\\secondColorPicX, \\secondColorPicY) {\\includegraphics[width=25px, height=10px]{Resources/SecondColor.png}};",
                       paste("\t\t\\node[inner sep=0, anchor=south west, align=center, right = 0.1 of secondColor] (secondColorLabel) {", titles[2], "};", sep=""),
                       "",
                       "\t\t% Draw legend box",
                       "\t\t\\draw let \\p1=(secondColorLabel.south east) in let \\p2=(firstColorLabel.south east) in let \\n1={max(\\x1,\\x2)} in  ($(emptyCircle.north west) + (-\\outerMarginArea, \\spaceBetweenLegendTitleAndLegend)$) rectangle ($(\\n1, \\y1) + (\\outerMarginArea, -\\outerMarginArea)$);",
                       "")
  } else {
    legendContent <- c(legendContent, 
                       "",
                       "\t\t% Draw legend box",
                       "\t\t\\draw let \\p1=(secondLeftLegendText.south east) in let \\p2=(firstColorLabel.south east) in ($(emptyCircle.north west) + (-\\outerMarginArea, \\spaceBetweenLegendTitleAndLegend)$) rectangle ($(\\x2, \\y1) + (\\outerMarginArea, -\\outerMarginArea)$);",
                       "")
  }
  
  content <- c(content,
               legendContent)
  
  content <- c(content, 
               c("\t\\end{tikzpicture}",
                "\\end{document}"
               ));
  
  
  fileConn <- file(filePath);
  
  writeLines(content, fileConn);
  
  close(fileConn);
}

### Convert supplied data into plottable format
  # (a) add abs(centre.y) to supplied plot data 
  #[creates plot centroid of 0,0 for internal use, regardless of min. value of y
  # in user-supplied data]
  plot.data.offset <- plot.data
  plot.data.offset[,2:ncol(plot.data)]<- plot.data[,2:ncol(plot.data)]+abs(centre.y)
  #print(plot.data.offset)
  # (b) convert into radial coords
  group <-NULL
  group$path <- CalculateGroupPath(plot.data.offset, plot.data)
  
  #print(group$path)
  # (c.1) Calculate coordinates required to plot radial variable axes
  axis <- NULL
  axis$path <- CalculateAxisPath(var.names,grid.min+abs(centre.y),grid.max+abs(centre.y))
  
  # (c.2) Compute the coordinates required to plot the lines to the text
  textLines <- NULL
  textLines$path <- CalculateAxisPath(var.names,grid.max+abs(centre.y), grid.max+abs(centre.y)+line.offset)
  
  # Split the labels of the axis for alignment
  splitLabels <- SplitInHalf(axis.labels)
  
  axis.labels <- c(breakText(splitLabels[[1]], includePreceedingPhantom=FALSE), breakText(splitLabels[[2]], rightAlign=TRUE, includePreceedingPhantom=FALSE));
  #print(axis$path)
  # (d) Create file containing axis labels + associated plotting coordinates
  #Labels
  axis$label <- data.frame(
    text=axis.labels,
    x=NA,
    y=NA )
  #print(axis$label)
  #axis label coordinates
  n.vars <- length(var.names)
  angles = seq(from=0, to=2*pi, by=(2*pi)/n.vars)
  axis$label$x <- sapply(1:n.vars, function(i, x) {((grid.max+abs(centre.y)+line.offset)*axis.label.offset)*sin(angles[i])})
  axis$label$y <- sapply(1:n.vars, function(i, x) {((grid.max+abs(centre.y)+line.offset)*axis.label.offset)*cos(angles[i])})
  #print(axis$label)
  # (e) Create Circular grid-lines + labels
  #caclulate the cooridinates required to plot circular grid-lines for three user-specified
  #y-axis values: min, mid and max [grid.min; grid.mid; grid.max]
  gridline <- NULL
  gridline$min$path <- funcCircleCoords(c(0,0),grid.min+abs(centre.y),npoints = 360)
  gridline$mid$path <- funcCircleCoords(c(0,0),grid.mid+abs(centre.y),npoints = 360)
  gridline$max$path <- funcCircleCoords(c(0,0),grid.max+abs(centre.y),npoints = 360)
  #print(head(gridline$max$path))
  #gridline labels
  gridline$min$label <- data.frame(x=gridline.label.offset,y=grid.min+abs(centre.y),
                                   text=as.character(grid.min))
  gridline$max$label <- data.frame(x=gridline.label.offset,y=grid.max+abs(centre.y),
                                   text=as.character(grid.max))
  gridline$mid$label <- data.frame(x=gridline.label.offset,y=grid.mid+abs(centre.y),
                                   text=as.character(grid.mid))
  #print(gridline$min$label)
  #print(gridline$max$label)
  #print(gridline$mid$label)
### Start building up the radar plot

# Delcare 'theme_clear', with or without a plot legend as required by user
#[default = no legend if only 1 group [path] being plotted]
theme_clear <- theme_bw(base_size=20) + 
  theme(axis.text.y=element_blank(),
        axis.text.x=element_blank(),
        axis.ticks=element_blank(),
        panel.grid.major=element_blank(),
        panel.grid.minor=element_blank(),
        panel.border=element_blank(),
        legend.key=element_rect(linetype="blank"))

if (plot.legend==FALSE) theme_clear <- theme_clear + theme(legend.position="none")

#Base-layer = axis labels + plot extent
# [need to declare plot extent as well, since the axis labels don't always
# fit within the plot area automatically calculated by ggplot, even if all
# included in first plot; and in any case the strategy followed here is to first
# plot right-justified labels for axis labels to left of Y axis for x< (-x.centre.range)], 
# then centred labels for axis labels almost immediately above/below x= 0 
# [abs(x) < x.centre.range]; then left-justified axis labels to right of Y axis [x>0].
# This building up the plot in layers doesn't allow ggplot to correctly 
# identify plot extent when plotting first (base) layer]

#base layer = axis labels for axes to left of central y-axis [x< -(x.centre.range)]
base <- ggplot(axis$label) + xlab(NULL) + ylab(NULL) + coord_equal() +
  #geom_text(data=subset(axis$label,axis$label$x < (-x.centre.range)),
  #          aes(x=x,y=y,label=text),size=axis.label.size,hjust=1, family=font.radar) +
  scale_x_continuous(limits=c(-2.5*plot.extent.x,2.5*plot.extent.x)) + 
  scale_y_continuous(limits=c(-1.5*plot.extent.y,1.5*plot.extent.y))

  # + axis labels for any vertical axes [abs(x)<=x.centre.range]
  #base <- base + geom_text(data=subset(axis$label,abs(axis$label$x)<=x.centre.range),
  #                         aes(x=x,y=y,label=text),size=axis.label.size,hjust=0.5, family=font.radar)
  # + axis labels for any vertical axes [x>x.centre.range]
  #base <- base + geom_text(data=subset(axis$label,axis$label$x>x.centre.range),
  #                         aes(x=x,y=y,label=text),size=axis.label.size,hjust=0, family=font.radar)

  # + theme_clear [to remove grey plot background, grid lines, axis tick marks and axis text]
  base <- base + theme_clear
  #  + background circle against which to plot radar data
  
  # The first cirlce is the outer one
  base <- base + geom_polygon(data=gridline$max$path,aes(x,y),
                              fill=background.circle.outer.colour,
                              alpha=background.circle.transparency)
  # For the inner circle, a white opaque circle is placed firstly
  base <- base + geom_polygon(data=gridline$mid$path,aes(x,y),
                              fill="white",
                              alpha=1)
  base <- base + geom_polygon(data=gridline$mid$path,aes(x,y),
                              fill=background.circle.inner.colour,
                              alpha=background.circle.transparency)
  # The innermost circle should remain white
  base <- base + geom_polygon(data=gridline$min$path,aes(x,y),
                              fill="white",
                              alpha=1)
  

  # + radial axes
  base <- base + geom_path(data=axis$path,aes(x=x,y=y,group=axis.no),
                           colour=axis.line.colour)
  
  base <- base + geom_path(data=textLines$path,aes(x=x,y=y,group=axis.no),
                           colour=I("black"))


  # ... + group (cluster) 'paths'
  for (k in 1:length(unique(group$path$group))) {
    groupName <- unique(group$path$group)[k];
    base <- base + geom_path(data=group$path[group$path$group == groupName,],aes(x=x,y=y,group=group,colour=group),
                            size=group.line.width)
  }

  # Filter the group points
  nonZero <- group$path;
  nonZero <- nonZero[nonZero$val != 0,];
  eqZero <- group$path;
  eqZero <- eqZero[group$path$val == 0,];
  
  browser();
  # ... + group points (cluster data)
  for (k in 1:length(unique(group$path$group))) {
    groupName <- unique(group$path$group)[k];
    
    if (nrow(eqZero[eqZero$group == groupName,]) > 0) {
      base <- base + geom_point(data=eqZero[eqZero$group == groupName,],aes(x=x,y=y,group=group,colour=group, fill=I("white")), shape=21, size=group.point.size)  
    }
    
    if (nrow(nonZero[nonZero$group == groupName,]) > 0) {
      base <- base + geom_point(data=nonZero[nonZero$group == groupName,],aes(x=x,y=y,group=group,colour=group), size=group.point.size)  
    }
  }

  #... + amend Legend title
  if (plot.legend==TRUE) base  <- base + labs(colour=legend.title,size=legend.text.size)
  # ... + circular grid-lines at 'min', 'mid' and 'max' y-axis values
  base <- base +  geom_path(data=gridline$min$path,aes(x=x,y=y),
                            lty=gridline.min.linetype,colour=gridline.min.colour,size=grid.line.width)
  base <- base +  geom_path(data=gridline$mid$path,aes(x=x,y=y),
                            lty=gridline.mid.linetype,colour=gridline.mid.colour,size=grid.line.width)
  base <- base +  geom_path(data=gridline$max$path,aes(x=x,y=y),
                            lty=gridline.max.linetype,colour=gridline.max.colour,size=grid.line.width)
  # ... + grid-line labels (max; ave; min) [only add min. gridline label if required]
  if (label.gridline.min==TRUE) {

  base <- base + geom_text(aes(x=x,y=y,label=values.radar[1]),data=gridline$min$label,size=grid.label.size*0.8, hjust=1, family=font.radar) 
  }
  base <- base + geom_text(aes(x=x,y=y,label=values.radar[2]),data=gridline$mid$label,size=grid.label.size*0.8, hjust=1, family=font.radar)
  base <- base + geom_text(aes(x=x,y=y,label=values.radar[3]),data=gridline$max$label,size=grid.label.size*0.8, hjust=1, family=font.radar)
  # ... + centre.y label if required [i.e. value of y at centre of plot circle]
  if (label.centre.y==TRUE) {
    centre.y.label <- data.frame(x=0, y=0, text=as.character(centre.y))
    base <- base + geom_text(aes(x=x,y=y,label=text),data=centre.y.label,size=grid.label.size, hjust=0.5, family=font.radar) }

  base <- base + theme(legend.key.width=unit(3,"line")) + theme(text = element_text(size = 20,
                                                                                      family = font.radar)) +
  theme(legend.text = element_text(size = legend.text.size), legend.position="none") +
  #theme(legend.box.background = element_rect(), legend.box.margin = margin(1,1,1,1)) + # add the box around the legend
  #theme(legend.key.height=unit(2,"line")) +
  scale_colour_manual(values=rep(c("#FFB400", "#4045FF", "#007A87",  "#8CE071", "#7B0051", 
    "#00D1C1", "#FFAA91", "#B4A76C", "#9CA299", "#565A5C", "#00A04B", "#E54C20"), 100)) +
  scale_fill_manual(name="", values=c("white"), labels="No occurence") #+
  #guides(colour=guide_legend(nrow=2,byrow=TRUE)) +
  #theme(text=element_text(family=font.radar)) + 
  #theme(legend.title=element_blank())
  
  GenerateTexFile("StarPlot.tex", "StarPlot_1.pdf", axis$label$text, plot.data[,1])

  return(base)

}