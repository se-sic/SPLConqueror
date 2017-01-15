import sys
import learning
import parameterTuning

CONF_MARKER = "Configurations"

# Messages received by the parent process
SETTING_STREAM_START = "settings_start"
SETTING_STREAM_END = "settings_end"

CONFIG_LEARN_STREAM_START = "config_learn_start"
CONFIG_LEARN_STREAM_END = "config_learn_end"

CONFIG_PREDICT_STREAM_START = "config_predict_start"

CONFIG_PREDICT_STREAM_END = "config_predict_end"

START_LEARN = "start_learn"
START_PARAM_TUNING = "start_param_tuning"

# Messages sent to the parent process
REQUESTING_CONFIGURATION = "req_configs"
REQUESTING_LEARNING_RESULTS = "req_results"

PASS_OK = "pass_ok"
FINISHED_LEARNING = "learn_finished"

REQUESTING_LEARNING_SETTINGS = "req_settings"


# Output finction to pass strings to C#, flushing the output buffer is required to make sure the string is written
#  in the stream
def print_line(string):
    print string
    # flushing output buffer
    sys.stdout.flush()


# format and print a list
def print_lineArray(array):
    output = ""
    for item in array:
        output += str(item)
        output += ","
    print output
    sys.stdout.flush()


# Function to request and then parse configurations.
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


# Request and return the configurations used to train.
def get_configurationsLearn(container):
    return get_configurations(container, CONFIG_LEARN_STREAM_START, CONFIG_LEARN_STREAM_END)


# Request and return the configurations used to preidct.
def get_configurationsPredict(container):
    return get_configurations(container, CONFIG_PREDICT_STREAM_START, CONFIG_PREDICT_STREAM_END)


# Main method, that will be executed when executing this script.
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

    task = raw_input()
    # perform prediction
    if task == START_LEARN:
        model = learning.Learner(learning_strategy, learner_settings)
        model.learn(configurationsLearn.features, configurationsLearn.results)
        predictions = model.predict(configurationsPredict.features)

        print_line(FINISHED_LEARNING)
        if raw_input() == REQUESTING_LEARNING_RESULTS:
            print_lineArray(predictions)
    # perform parameter tuning
    elif task == START_PARAM_TUNING:
        print_line(FINISHED_LEARNING)
        target_path = raw_input()
        parameterTuning.setOutputPath(target_path)
        optimalParameters = parameterTuning.optimizeParameter(learning_strategy, configurationsLearn.features,
                                                              configurationsLearn.results, learner_settings)
        if raw_input() == REQUESTING_LEARNING_RESULTS:
            print_line(optimalParameters)


# class to hold values passed by c#
class Configurations:
    def __init__(self):
        self.results = []
        self.features = []

    def append(self, nfp_value, features):
        self.results.append(nfp_value)
        self.features.append(features)

main()


def test_function():
    model = learning.Learner("svr", "")
    model.learn([[0, 0]], [1])
    pred = model.predict([[0, 0]])
    print_lineArray(pred)
    model = learning.Learner("ssvr", "")
    model.learn([[0, 0]], [1])
    pred = model.predict([[0, 0]])
    print_lineArray(pred)
    model = learning.Learner("svr", ["C=\'bogus\'"])
    model.learn([[0, 0]], [1])
    print_lineArray(model.predict([[0, 0]]))
    model = learning.Learner("svr", [])
    print_lineArray(model.predict([[0, 0]]))
