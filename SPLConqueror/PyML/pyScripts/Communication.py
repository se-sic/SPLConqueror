import sys
import learning
import parameterTuning
import configParser
import learnerExtraction
from sklearn.tree import DecisionTreeRegressor as DTR
from sklearn.ensemble.forest import RandomForestRegressor as RF
from sklearn.svm import SVR
from sys import argv
from time import perf_counter


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

debug = False

# Output function to pass strings to C#, flushing the output buffer is required to make sure the string is written
#  in the stream
def print_line(string):
    print(string)
    # flushing output buffer
    sys.stdout.flush()

def check_prereq(model):
    return isinstance(model, DTR) or isinstance(model, RF) or (isinstance(model, SVR) and model.kernel=="linear")


# format and print a list
def print_line_array(array):
    output = ""
    for item in array:
        output += str(item)
        output += ","
    print(output)
    sys.stdout.flush()


tree_path = ""


# Function to request and then parse configurations.
def get_configurations(learn_container, predict_container):
    print_line(REQUESTING_CONFIGURATION)
    line = input()
    config_and_nfp_file_learn = line.split(" ")

    print_line(PASS_OK)
    line = input()
    config_and_nfp_file_predict = line.split(" ")
    print_line(PASS_OK)
    global tree_path
    tree_path = input()
    print_line(PASS_OK)
    options = input()
    if config_and_nfp_file_learn[0].strip().endswith(".csv"):
        data = configParser.parse_from_csv(config_and_nfp_file_learn[0].strip(), config_and_nfp_file_predict[0].strip(),
                                           config_and_nfp_file_learn[1].strip(), config_and_nfp_file_predict[1].strip())
        learn_container.conf_file = config_and_nfp_file_learn[0].strip()
        learn_container.nfp_file = config_and_nfp_file_learn[1].strip()
        predict_container.conf_file = config_and_nfp_file_predict[0].strip()
        predict_container.nfp_file = config_and_nfp_file_predict[1].strip()
    else:
        data = configParser.parse_from_plain_text(options.strip().split(","), config_and_nfp_file_learn[0].strip(),
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

    if argv[1].lower() == "true":
        global debug
        debug = True
        print("Debug output mode enabled. \n Processing input\n", file=sys.stderr, flush=True)

    configurations_learn = Configurations()
    configurations_predict = Configurations()

    learning_strategy = ""
    learner_settings = []

    # Sequence for getting the basic learning settings from C#
    print_line(REQUESTING_LEARNING_SETTINGS)
    csharp_response = input()
    if csharp_response == SETTING_STREAM_START:
        learning_strategy = input()
        learner_setting = input()

        if debug:
            print("Received learning strategy:" + learning_strategy + "\n", file=sys.stderr, flush=True)
            print("Received learner setting:" + learner_setting + "\n", file=sys.stderr, flush=True)

        while learner_setting != SETTING_STREAM_END:
            # pair of settings passed by other application in format identifier=value
            learner_settings.append(learner_setting)
            learner_setting = input()

            if debug:
                print("Received learner setting:" + learner_setting + "\n", file=sys.stderr, flush=True)

    get_configurations(configurations_learn, configurations_predict)

    if debug:
        print("Found learning set. " + str(configurations_learn) + "\n", file=sys.stderr, flush=True)
        print("Found validation set. " + str(configurations_learn) + "\n", file=sys.stderr, flush=True)

    task = input()
    # perform prediction
    if task == START_LEARN:

        if debug:
            print("Starting the learning.\n", file=sys.stderr, flush=True)

        learning.number_of_configurations = len(configurations_learn.results)
        model = learning.Learner(learning_strategy, learner_settings)
        start = perf_counter()
        model.learn(configurations_learn.features, configurations_learn.results)
        elapsed = perf_counter() - start
        predictions = model.predict(configurations_predict.features)

        print_line(FINISHED_LEARNING)

        if debug:
            print("Finished the learning.\n", file=sys.stderr, flush=True)

        if input() == REQUESTING_LEARNING_RESULTS:
            if debug:
                print("Extracting trees.\n", file=sys.stderr, flush=True)
            print_line_array(predictions)
        if not tree_path.strip() is "" and check_prereq(model.learning_model):
            print_line(str(elapsed))
            tree_file = open(tree_path, 'w')
            tree = learnerExtraction.extract(model.learning_model)
            if len(tree) == 1:
                tree_file.write(str(tree) + "\n")
            else:
                forest = tree
                for tree in forest:
                    tree_file.write(str(tree) + "\n")
            tree_file.flush()
            tree_file.close()

    # perform parameter tuning
    elif task == START_PARAM_TUNING:
        if debug:
            print("Starting the learning.\n", file=sys.stderr, flush=True)
        parameterTuning.setOutputPath(input())
        optimal_parameters = parameterTuning.optimizeParameter(learning_strategy, configurations_learn.features,
                                                               configurations_learn.results, learner_settings)
        print_line(FINISHED_LEARNING)
        if input() == REQUESTING_LEARNING_RESULTS:
            print_line(optimal_parameters)


# class to hold values passed by c#
class Configurations:
    def __init__(self):
        self.results = []
        self.features = []
        self.conf_file = ""
        self.nfp_file = ""

    def append(self, nfp_value, features):
        self.results.append(nfp_value)
        self.features.append(features)

    def __str__(self):
        return "Configurations from file " + self.conf_file + " with nfps " + self.nfp_file + "."

main()
