using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class stores all information that are needed to perform multiple experiments with one case study. 
    /// </summary>
    public class GlobalState
    {
        /// <summary>
        /// The logger instance that is used to log general information.
        /// </summary>
        public static Logger logInfo = new InfoLogger(null);

        /// <summary>
        /// The logger instance that is used to log error messages.
        /// </summary>
        public static Logger logError = new ErrorLogger(null);

        /// <summary>
        /// The variability model of the case study. 
        /// </summary>
        public static VariabilityModel varModel = null;
        /// <summary>
        /// Path the variablity model is located.
        /// </summary>
        public static string vmSource = null;

        /// <summary>
        /// The property being considered. 
        /// </summary>
        public static NFProperty currentNFP = NFProperty.DefaultProperty;

        /// <summary>
        /// This object provides access to all measurements of the case study.
        /// </summary>
        public static ResultDB allMeasurements = new ResultDB();

        /// <summary>
        /// This object provides access to the set of measurements that is used for the evaluation.
        /// </summary>
        public static ResultDB evaluationSet = new ResultDB();

        /// <summary>
        /// The deviation of the measurements.
        /// </summary>
        public static double measurementDeviation = Double.MinValue;

        /// <summary>
        /// Path the measurements are located at.
        /// </summary>
        public static string measurementSource;

        /// <summary>
        /// The object that encapsulates the configurations that can be used to evalue the learned predictor.
        /// </summary>
        public static ResultDB evalutionSet = new ResultDB();

        /// <summary>
        /// If we require a configuration for learning, but haven't measured it, shall we use a similar one instead?
        /// </summary>
        public static bool takeSimilarConfig = true;

        /// <summary>
        /// Indication that the system is currently trying to recover a previous state.
        /// </summary>
        public static bool rollback = false;

        /// <summary>
        /// All properties of the current case study. 
        /// </summary>
        public static Dictionary<string, NFProperty> nfProperties = new Dictionary<string,NFProperty>();

        private static Dictionary<Configuration, Configuration> substitutedConfigs = new Dictionary<Configuration, Configuration>();

        /// <summary>
        /// An partial or full order of the configuration options, that is used in the write configuration command.
        /// </summary>
        public static List<ConfigurationOption> optionOrder = new List<ConfigurationOption>();

        private GlobalState(){ }

        /// <summary>
        /// Clears the global state. This mehtod should be used after performing all experiments of one case study. 
        /// </summary>
        public static void clear()
        {
            Logger tempInf = logInfo;
            Logger tempErr = logError;
            ConstructorInfo constructor = typeof(GlobalState).GetConstructor(BindingFlags.Static | BindingFlags.NonPublic, null, new Type[0], null);
            constructor.Invoke(null, null);
            logInfo = tempInf;
            logError = tempErr;
        }


        /// <summary>
        /// The mehtod returns non function property with the given name. If there is no property with the name, a new property is created. 
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>A non functional property with the specified name.</returns>
        public static NFProperty getOrCreateProperty(string name)
        {
            if(nfProperties.Keys.Contains(name))
                return nfProperties[name];
            else{
                NFProperty newProp = new NFProperty(name);
                nfProperties.Add(name, newProp);
                return newProp;
            }
        }

        /// <summary>
        /// Adds a configration to the global state. 
        /// </summary>
        /// <param name="config">An configuration of the variability model.</param>
        public static void addConfiguration(Configuration config)
        {
            GlobalState.allMeasurements.add(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public static void setDefaultProperty(String propertyName)
        {
            GlobalState.currentNFP = GlobalState.nfProperties[propertyName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public static void setDefaultProperty(NFProperty property)
        {
            GlobalState.currentNFP = property;
        }

        /// <summary>
        /// This function gets a list of configurations and checks in the global state whether this configuration has a measured value and returns it if so.
        /// </summary>
        /// <param name="list">The list of configurations for which we want a measured value.</param>
        /// <returns>A list of configurations containinga measured value. Might be empty.</returns>
        public static List<Configuration> getMeasuredConfigs(List<Configuration> list)
        {
            List<Configuration> configsWithValues = new List<Configuration>();
            List<Task<Tuple<Configuration, Configuration>>> taskList = new List<Task<Tuple<Configuration, Configuration>>>();

            foreach (Configuration c in list)
            {
                if (substitutedConfigs.Keys.Contains(c))
                {
                    configsWithValues.Add(substitutedConfigs[c]);
                    continue;
                }
                var customCulture = Thread.CurrentThread.CurrentCulture;
                Task<Tuple<Configuration, Configuration>> task = Task.Factory.StartNew( (Object con) =>
                {
                    Configuration conf = (Configuration)con;

                    Thread.CurrentThread.CurrentCulture = customCulture;
                    return getSimilarConfig(conf);
                }, c ) ;

                //var taskA = Task<Tuple<Configuration, Configuration>>.Factory.StartNew(() => getSimilarConfig(config)); // TDOD
                taskList.Add(task);
            }
            Task.WaitAll(taskList.ToArray());

            int found = 0;

            while (taskList.Count > 0)
            {
                int i = Task.WaitAny(taskList.ToArray());

                Task<Tuple<Configuration, Configuration>> task = taskList[i];
                Tuple<Configuration, Configuration> result = task.Result;

                found++;

                taskList.Remove(task);

               

                if (result.Item2 != null && !substitutedConfigs.ContainsKey(result.Item1))
                {

                    substitutedConfigs.Add(result.Item1, result.Item2);
                    configsWithValues.Add(result.Item2);
                }
            }
            return configsWithValues;
        }

        private static Tuple<Configuration, Configuration> getSimilarConfig(Configuration config)
        {
            Configuration similarConfig = null;
            List<Configuration> similarOnes = new List<Configuration>();
            int nbCount = config.BinaryOptions.Count;
            if (config.BinaryOptions.Keys.Contains(varModel.Root))
                nbCount--;
            bool found = false;
            foreach (var configInGS in GlobalState.allMeasurements.getSimilarConfigs(config))
            {
                if (config.Equals(configInGS))
                {
                    similarConfig = configInGS;
                    found = true;
                    break;
                }
                else if (takeSimilarConfig)
                {
                    var conf = findSimilarConfigBinary(config, configInGS, nbCount);
                    if (conf != null)
                        similarOnes.Add(conf);
                }
            }
            if (!found)
            {
                if (takeSimilarConfig && similarOnes.Count > 0)
                    similarConfig = findSimilarConfigNumeric(config, similarOnes);
                else
                {
                    if (similarOnes.Count == 0)
                        logError.logLine("Required config: " + config.ToString() + " " + config.printConfigurationForMeasurement());
                }
            }

            return Tuple.Create<Configuration, Configuration>(config, similarConfig);
        }

        private static Configuration findSimilarConfigNumeric(Configuration config, List<Configuration> similarOnes)
        {            int minDistance = Int32.MaxValue;
            Configuration best = null;
            foreach (var conf in similarOnes)
            {
                int distance = 0;
                foreach (var numOpt in conf.NumericOptions.Keys)
                {
                    if (allMeasurements.blacklisted != null &&
                        allMeasurements.blacklisted.Contains(numOpt.Name.ToLower()))
                    {
                        continue;
                    }
                    if (config.NumericOptions[numOpt] == conf.NumericOptions[numOpt])
                        continue;            
                    double valDist = Math.Abs(config.NumericOptions[numOpt] - conf.NumericOptions[numOpt]);
                    distance += numOpt.getStepFast(valDist + numOpt.Min_value);
                    if (distance > minDistance)
                        break;
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    best = conf;
                }
            }
            return best;
        }

        private static Tuple<Configuration, int> calculateDistance(Configuration config, Configuration conf)
        {
            int distance = 0;
            foreach (var numOpt in conf.NumericOptions.Keys)
            {
                if (config.NumericOptions[numOpt] == conf.NumericOptions[numOpt])
                    continue;
                double valDist = Math.Abs(config.NumericOptions[numOpt] - conf.NumericOptions[numOpt]);
                distance += numOpt.getStepFast(valDist + numOpt.Min_value);
                //distance += Math.Abs(stepInValueRange[numOpt] - numOpt.getStep(conf.NumericOptions[numOpt]));
                if (distance > Int32.MaxValue)
                    break;
            }

            return Tuple.Create<Configuration, int>(conf ,distance);
        }

        private static Configuration findSimilarConfigBinary (Configuration config, Configuration configInGS, int nbCount)
        {
            int nbCount2 = configInGS.BinaryOptions.Count;
            if (configInGS.BinaryOptions.Keys.Contains (varModel.Root))
                nbCount2--;
            if (nbCount != nbCount2)
                return null;
            foreach (var binOpt in config.BinaryOptions.Keys) {
                if (binOpt == varModel.Root)
                    continue;
                if (!configInGS.BinaryOptions.Keys.Contains (binOpt)) {
                    return null;
                }
            }
            return configInGS;
        }
    }
}
