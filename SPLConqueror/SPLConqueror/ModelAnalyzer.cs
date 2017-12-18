using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class ModelAnalyzer
    {

        VariabilityModel varModelToAnalyze;

        /// <summary>
        /// Creates a new instance to analyze the given model.
        /// </summary>
        /// <param name="varModel">The variability model to analyze.</param>
        public ModelAnalyzer(VariabilityModel varModel)
        {
            this.varModelToAnalyze = varModel;
        }

        public void analyzeModel()
        {
            GlobalState.logInfo.logLine("Number binary options.");
            GlobalState.logInfo.logLine("Number binary options: "+ varModelToAnalyze.BinaryOptions.Count);

            GlobalState.logInfo.logLine("Number of optional options.");
            GlobalState.logInfo.logLine("Number optional options: " + numOptOptions());

            GlobalState.logInfo.logLine("Number of cross-tree constraints among binary options");
            GlobalState.logInfo.logLine("Number of binary ctc: "+ numCrossTreeConstraints_Binary());

            GlobalState.logInfo.logLine("Max branching factor (maximal number of binary options with the same parent option).");
            GlobalState.logInfo.logLine("Max branching factor: "+maxBranchingFactor());

            GlobalState.logInfo.logLine("Average branching factor (average number of binary options with the same parent option).");
            GlobalState.logInfo.logLine("Average branching factor: " + avgBranchingFactor());

            GlobalState.logInfo.logLine("Max depth of the variability model (longest parent-child relationship).");
            GlobalState.logInfo.logLine("Max depth: " + maxModelDepth());

            GlobalState.logInfo.logLine("Avg depth of the variability model (average parent-child relationship).");
            GlobalState.logInfo.logLine("Avg depth: " + avgModelDepth());

            GlobalState.logInfo.logLine("Number of alternative groups.");
            GlobalState.logInfo.logLine("Number alternative groups: " + numberAlternativeGroups());

            GlobalState.logInfo.logLine("Size of the largest alternative group.");
            GlobalState.logInfo.logLine("Size largest alternative group: " + maxSizeOfAlterantiveGroups());

            GlobalState.logInfo.logLine("Average size of the alternative groups.");
            GlobalState.logInfo.logLine("Average size of alternative groups: " + avgSizeOfAlternativeGroups());

        }

        private int numberAlternativeGroups()
        {
            int number = 0;

            foreach (BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                if (bin.isParentOfAlternativeGroup())
                {
                    number += 1;
                }
            }
            return number;
        }

        private int maxSizeOfAlterantiveGroups()
        {
            int maxSize = 0;

            foreach(BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                if (bin.isParentOfAlternativeGroup())
                {
                    maxSize = Math.Max(maxSize, bin.Children.Count);
                }
            }
            return maxSize;
        }

        private double avgSizeOfAlternativeGroups()
        {
            int sumSize = 0;
            int numberAltGroups = 0;

            foreach (BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                if (bin.isParentOfAlternativeGroup())
                {
                    numberAltGroups += 1;
                    sumSize += bin.Children.Count;
                }
            }

            if (numberAltGroups == 0)
                return 0;

            return ((double)sumSize) / ((double)numberAltGroups);

        }



        private double avgBranchingFactor()
        {
            int sumBranch = 0;

            foreach (BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                foreach (BinaryOption other in varModelToAnalyze.BinaryOptions)
                {
                    if (other.Parent == bin)
                    {
                        sumBranch += 1;
                    }
                }
               
            }
            return ((double)sumBranch) / ((double)varModelToAnalyze.BinaryOptions.Count);
        }

        private int maxBranchingFactor()
        {
            int maxBranch = 0;

            foreach (BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                int currBranch = 0;

                foreach (BinaryOption other in varModelToAnalyze.BinaryOptions)
                {
                    if(other.Parent == bin)
                    {
                        currBranch += 1;
                    }
                }
                maxBranch = Math.Max(currBranch, maxBranch);

            }
            return maxBranch;

        }

        private int numCrossTreeConstraints_Binary()
        {
            return varModelToAnalyze.BinaryConstraints.Count;
        }

        private int numOptOptions()
        {
            int numOpt = 0;
            foreach(BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                if (bin.Optional)
                {
                    numOpt += 1;
                }
            }
            return numOpt;
        }

        private int maxModelDepth()
        {
            int maxDepth = 0;

            foreach( BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                int currDepth = 0;
                ConfigurationOption parent = bin.Parent;
                while(parent != null)
                {
                    parent = parent.Parent;
                    currDepth += 1;
                }
                maxDepth = Math.Max(currDepth, maxDepth);

            }
            return maxDepth;
        }

        private double avgModelDepth()
        {
            int allDepth = 0;

            foreach (BinaryOption bin in varModelToAnalyze.BinaryOptions)
            {
                ConfigurationOption parent = bin.Parent;
                while (parent != null)
                {
                    parent = parent.Parent;
                    allDepth += 1;
                }
            }
            return ((double)allDepth)/((double)varModelToAnalyze.BinaryOptions.Count);
        }


    }
}
