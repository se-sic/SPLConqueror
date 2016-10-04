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

def get_configurations(container, stream_start_arg, stream_end_arg):
    print_line(REQUESTING_CONFIGURATION)
    marker = raw_input()
    if marker == stream_start_arg:
        line = raw_input()
        while not line == stream_end_arg:

            # parse each line written by C# in the format binOpt,...,binOpt,numOpt,...,numOpt, nfp_value
            # into a list containing the option values as int and cast the nfp value to double
            # then save it in the configuration class
            data = line.split(",")
            nfp_value = float(data.pop())
            configuration_values = []
            for value in data:
                configuration_values.append(int(value))
            container.append(nfp_value, configuration_values)

            # message to C# indicating that current line is processed and next can be sent in order to control traffic
            print_line(PASS_OK)
            line = raw_input()

    return container

def get_configurationsLearn(container):
    return get_configurations(container, CONFIG_LEARN_STREAM_START, CONFIG_LEARN_STREAM_END)

def get_configurationsPredict(container):
    return get_configurations(container, CONFIG_PREDICT_STREAM_START, CONFIG_PREDICT_STREAM_END)
    
def main():
    configurationsLearn = Configurations()
    configurationsPredict = Configurations()
    learning_strategy = ""
    learner_settings = []

    # Sequenz for getting the basic learning settings from C#
    print_line(REQUESTING_LEARNING_SETTINGS)
    csharp_response = raw_input()
    if csharp_response == SETTING_STREAM_START:
        learning_strategy = raw_input()
        learner_setting = raw_input()
        while learner_setting != SETTING_STREAM_END:
            # pair of settings passed by other application in format identifier=value
            learner_settings.append(learner_setting)
            learner_setting = raw_input()
    
    configurationsLearn = get_configurationsLearn(configurationsLearn)
   
    configurationsPredict = get_configurationsPredict(configurationsPredict)

    model = learning.Learner(learning_strategy, learner_settings)
    model.learn(configurationsLearn.features, configurationsLearn.results)
    predictions = model.predict(configurationsPredict.features)
    
    
    print_line(FINISHED_LEARNING)
    if raw_input() == REQUESTING_LEARNING_RESULTS:
        print_lineArray(predictions)

# class to hold values passed by c#
class Configurations():

    def __init__(self):
        self.results = []
        self.features = []

    def append(self, nfp_value, features):
        self.results.append(nfp_value)
        self.features.append(features)

main()


