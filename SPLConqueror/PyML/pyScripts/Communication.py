import sys
import learning
import parameterTuning
import parser

# Messages received by the parent process
SETTING_STREAM_START = "settings_start"
SETTING_STREAM_END = "settings_end"

START_LEARN = "start_learn"
START_PARAM_TUNING = "start_param_tuning"

# Messages sent to the parent process
REQUESTING_CONFIGURATION = "req_configs"
REQUESTING_LEARNING_RESULTS = "req_results"

PASS_OK = "pass_ok"
FINISHED_LEARNING = "learn_finished"

REQUESTING_LEARNING_SETTINGS = "req_settings"

number_of_configurations = 0


# Output function to pass strings to C#, flushing the output buffer is required to make sure the string is written
#  in the stream
def print_line(string):
    print(string)
    # flushing output buffer
    sys.stdout.flush()


# format and print a list
def print_line_array(array):
    output = ""
    for item in array:
        output += str(item)
        output += ","
    print(output)
    sys.stdout.flush()


# Function to request and then parse configurations.
def get_configurations(learn_container, predict_container):
    print_line(REQUESTING_CONFIGURATION)
    line = raw_input()
    config_and_nfp_file_learn = line.split(" ")

    print_line(PASS_OK)
    line = raw_input()
    config_and_nfp_file_predict = line.split(" ")
    print_line(PASS_OK)
    options = raw_input()
    if config_and_nfp_file_learn[0].strip().endswith(".csv"):
        data = parser.parse_from_csv(config_and_nfp_file_learn[0].strip(), config_and_nfp_file_predict[0].strip(),
                                     config_and_nfp_file_learn[1].strip(), config_and_nfp_file_predict[1].strip())
    else:
        data = parser.parse_from_plain_text(options.strip().split(","), config_and_nfp_file_learn[0].strip(),
                                            config_and_nfp_file_predict[0].strip(),
                                            config_and_nfp_file_learn[1].strip(),
                                            config_and_nfp_file_predict[1].strip())
    learn_container.features = data[0]
    learn_container.results = data[2]
    predict_container.features = data[1]
    predict_container.results = data[3]
    print_line(PASS_OK)


# Main method, that will be executed when executing this script.
def main():
    configurations_learn = Configurations()
    configurations_predict = Configurations()

    learning_strategy = ""
    learner_settings = []

    # Sequence for getting the basic learning settings from C#
    print_line(REQUESTING_LEARNING_SETTINGS)
    csharp_response = raw_input()
    if csharp_response == SETTING_STREAM_START:
        learning_strategy = raw_input()
        learner_setting = raw_input()
        while learner_setting != SETTING_STREAM_END:
            # pair of settings passed by other application in format identifier=value
            learner_settings.append(learner_setting)
            learner_setting = raw_input()

    get_configurations(configurations_learn, configurations_predict)
    task = raw_input()
    # perform prediction
    if task == START_LEARN:

        global number_of_configurations
        number_of_configurations = len(configurations_learn.results)
        model = learning.Learner(learning_strategy, learner_settings)
        model.learn(configurations_learn.features, configurations_learn.results)
        predictions = model.predict(configurations_predict.features)

        print_line(FINISHED_LEARNING)
        if raw_input() == REQUESTING_LEARNING_RESULTS:
            print_line_array(predictions)
    # perform parameter tuning
    elif task == START_PARAM_TUNING:
        parameterTuning.setOutputPath(raw_input())
        optimal_parameters = parameterTuning.optimizeParameter(learning_strategy, configurations_learn.features,
                                                               configurations_learn.results, learner_settings)
        print_line(FINISHED_LEARNING)
        if raw_input() == REQUESTING_LEARNING_RESULTS:
            print_line(optimal_parameters)


# class to hold values passed by c#
class Configurations:
    def __init__(self):
        self.results = []
        self.features = []

    def append(self, nfp_value, features):
        self.results.append(nfp_value)
        self.features.append(features)

main()
