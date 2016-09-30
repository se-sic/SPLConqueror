import sys
import learning

CONF_MARKER = "Configurations"
REQUESTING_LEARNING_SETTINGS = "req_settings"
SETTING_STREAM_START = "settings_start"
SETTING_STREAM_END = "settings_end"
REQUESTING_CONFIGURATION = "req_configs"
REQUESTING_LEARNING_RESULTS = "req_results"

CONFIG_LEARN_STREAM_START = "config_learn_start"
CONFIG_LEARN_STREAM_END = "config_learn_end"

CONFIG_PREDICT_STREAM_START = "config_predict_start"

CONFIG_PREDICT_STREAM_END = "config_predict_end"

PASS_OK = "pass_ok"
FINISHED_LEARNING = "learn_finished"

# Output finction to pass strings to C#, flushing the output buffer is required to make sure the string is written in the stream
def print_line(string):
    print string
    # flushing output buffer
    sys.stdout.flush()
    
def print_lineArray(array):
    output = ""
    for item in array:
        output+=str(item)
        output+=","
    print output
    sys.stdout.flush()    

def parse_to_config(string):
            data = string.split(",")
            int_data = []
            nfp_val = float(data.pop())
            for option in data:
                int_data.append(int(option))
            return Configuration(int_data, nfp_val)

def get_configurationsLearn():
    configurations = []
    print_line(REQUESTING_CONFIGURATION)
    marker = raw_input()
    if marker == CONFIG_LEARN_STREAM_START:
        line = raw_input()
        while not line == CONFIG_LEARN_STREAM_END:
            config = parse_to_config(line)
            configurations.append(config)
            print_line(PASS_OK)
            line = raw_input()
    return configurations

def get_configurationsPredict():
    configurations = []
    print_line(REQUESTING_CONFIGURATION)
    marker = raw_input()
    if marker == CONFIG_PREDICT_STREAM_START:
        line = raw_input()
        while not line == CONFIG_PREDICT_STREAM_END:
            config = parse_to_config(line)
            configurations.append(config)
            print_line(PASS_OK)
            line = raw_input()
    return configurations    
    
def main():
    configurationsLearn = []
    configurationsPredict = []
    learning_strategy = ""
    kernel_settings = ""

    print_line(REQUESTING_LEARNING_SETTINGS)
    marker = raw_input()

    if marker == SETTING_STREAM_START:
        learning_strategy = raw_input()
        kernel_settings = raw_input()
    marker = raw_input()
    if marker == SETTING_STREAM_END:
        configurationsLearn = get_configurationsLearn()
   
    configurationsPredict = get_configurationsPredict()
    
    
    featuresLearn = []
    resultsLearn = []
    for config in configurationsLearn:
        featuresLearn.append(config.configuration_settings)
        resultsLearn.append(config.nfp_value)
	   
    featuresPredict = []
    resultsPredict = []
    for config in configurationsPredict:
        featuresPredict.append(config.configuration_settings)
        resultsPredict.append(config.nfp_value)	   
	   
    model = learning.learn(learning_strategy, kernel_settings, featuresLearn, resultsLearn)
    
    predictions = learning.predict(learning_strategy, model, featuresPredict, resultsPredict)
    
    
    print_line(FINISHED_LEARNING)
    if raw_input() == REQUESTING_LEARNING_RESULTS:
        print_lineArray(predictions)

class Configuration():

    def __init__(self, configuration_settings, nfp_value):
        self.nfp_value = nfp_value
        self.configuration_settings = configuration_settings

main()



