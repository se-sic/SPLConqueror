import sys
import learning

CONF_MARKER = "Configurations"
REQUESTING_LEARNING_SETTINGS = "req_settings"
SETTING_STREAM_START = "settings_start"
SETTING_STREAM_END = "settings_end"
REQUESTING_CONFIGURATION = "req_configs"
REQUESTING_LEARNING_RESULTS = "req_results"
CONFIG_STREAM_START = "config_start"
CONFIG_STREAM_END = "config_end"
PASS_OK = "pass_ok"
FINISHED_LEARNING = "learn_finished"

# Output finction to pass strings to C#, flushing the output buffer is required to make sure the string is written in the stream
def print_line(string):
    print string
    # flushing output buffer
    sys.stdout.flush()

def parse_to_config(string):
            data = string.split(",")
            int_data = []
            nfp_val = float(data.pop())
            for option in data:
                int_data.append(int(option))
            return Configuration(int_data, nfp_val)

def get_configurations():
    configurations = []
    print_line(REQUESTING_CONFIGURATION)
    marker = raw_input()
    if marker == CONFIG_STREAM_START:
        line = raw_input()
        while not line == CONFIG_STREAM_END:
            config = parse_to_config(line)
            configurations.append(config)
            print_line(PASS_OK)
            line = raw_input()
    return configurations

def main():
    configurations = []
    learning_strategy = ""
    kernel_settings = ""

    print_line(REQUESTING_LEARNING_SETTINGS)
    marker = raw_input()

    if marker == SETTING_STREAM_START:
        learning_strategy = raw_input()
        kernel_settings = raw_input()
    marker = raw_input()
    if marker == SETTING_STREAM_END:
        configurations = get_configurations()
    features = []
    results = []
    for config in configurations:
        features.append(config.configuration_settings)
        results.append(config.nfp_value)
    learning_result = learning.learn(learning_strategy, kernel_settings, features, results)
    print_line(FINISHED_LEARNING)
    if raw_input() == REQUESTING_LEARNING_RESULTS:
        print_line(learning_result)

class Configuration():

    def __init__(self, configuration_settings, nfp_value):
        self.nfp_value = nfp_value
        self.configuration_settings = configuration_settings

main()



