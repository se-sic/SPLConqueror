using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MachineLearning.Learning;
using MachineLearning;
using SPLConqueror_Core;
using MachineLearning.Learning.Regression;

namespace MachineLearning
{
    public class LearningInfo
    {

        public String learningApproach = "";

        public ML_Settings mlSettings = new ML_Settings();

        public string binarySamplings_Learning = "";

        public string binarySamplings_Validation = "";


        public string numericSamplings_Learning = "";

        public string numericSamplings_Validation = "";
        
        

        /// <summary>
        /// Returns a textual representation of this object consisting of the names of numerical and binary sampling methods performed for this experimental state and a representation of the mlsettings. 
        /// </summary>
        /// <returns>The textual reprentation.</returns>
        public override string ToString()    
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Learning: " + binarySamplings_Learning + "  " + numericSamplings_Learning + " using " + learningApproach);
            sb.Append(mlSettings.ToString());
 	        return sb.ToString();
        }

        /// <summary>
        /// Adds the identifier of a binary sampling method used for the learning set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addBinarySampling_Learning(string name)
        {
            binarySamplings_Learning += name + " ";
        }


        /// <summary>
        /// Adds the identifier of a numeric sampling method used for the learning set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addNumericSampling_Learning(string name)
        {
            numericSamplings_Learning += name + " ";
        }

        /// <summary>
        /// Adds the identifier of a binary sampling method used for the validation set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addBinarySampling_Validation(string name)
        {
            binarySamplings_Validation += name + " ";
        }


        /// <summary>
        /// Adds the identifier of a numeric sampling method used for the validation set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addNumericSampling_Validation(string name)
        {
            numericSamplings_Validation += name + " ";
        }



        /// <summary>
        /// Clears the binary and numeric selections stored in this object. 
        /// </summary>
        public void clearSampling()
        {
            learningApproach = "";
            binarySamplings_Learning = "";
            binarySamplings_Validation = "";
            numericSamplings_Learning = "";
            numericSamplings_Validation = "";
           
            
        }

        /// <summary>
        /// Clears the binary and numeric selections and the machine learning settings stored in this object. 
        /// </summary>
        public void clear()
        {
            mlSettings = new ML_Settings();
            clearSampling();
        }

    }
}
