import csv


def parse_configs_from_plaintext(configuration_file, configuration_options):
    f = open(configuration_file, 'r')
    config_selection = []

    for configuration in f:
        if not len(configuration.strp()) == 0:
            selection = []
            for option in configuration_options:
                if option + ";" in configuration:
                    selection.append(int(configuration.split(option + ";")[1].split("%;%")[0]))
                elif option in configuration:
                    selection.append(1)
                else:
                    selection.append(0)
            config_selection.append(selection)

    f.close()

    return config_selection


def parse_nfp_values(nfp_file):
    nfp_values = []

    f = open(nfp_file, 'r')
    for val in f:
        if not len(val.strip()) == 0:
            nfp_values.append(float(val))

    f.close()
    return nfp_values


def parse_from_plain_text(configuration_options, configuration_learn_file, configuration_predict_file, nfp_learn_file, 
                          nfp_predict_file):
    nfp_learn_values = parse_nfp_values(nfp_learn_file)
    nfp_predict_values = parse_nfp_values(nfp_predict_file)
    config_selection_learn = parse_configs_from_plaintext(configuration_learn_file, configuration_options)
    config_selection_predict = parse_configs_from_plaintext(configuration_predict_file, configuration_options)
    
    if (not len(nfp_learn_values) == len(config_selection_learn)) \
            or (not len(config_selection_predict) == len(nfp_predict_values)):
        raise RuntimeError

    return config_selection_learn, config_selection_predict, nfp_learn_values, nfp_predict_values


def parse_configs_from_csv(configuration_file):
    config_selection = []
    f = open(configuration_file, 'r')
    j = 0
    for row in csv.reader(f, delimiter=';'):
        if j == 0:
            j += 1
        else:
            config = []
            i = 0
            while i < len(row) - 1:
                config.append(int(row[i]))
                i += 1
            config_selection.append(config)

    f.close()
    return config_selection


def parse_from_csv(configuration_learn_file, configuration_predict_file, nfp_learn_file, nfp_predict_file):
    nfp_learn_values = parse_nfp_values(nfp_learn_file)
    nfp_predict_values = parse_nfp_values(nfp_predict_file)
    config_selection_learn = parse_configs_from_csv(configuration_learn_file)
    config_selection_predict = parse_configs_from_csv(configuration_predict_file)

    if (not len(nfp_learn_values) == len(config_selection_learn)) \
            or (not len(config_selection_predict) == len(nfp_predict_values)):
        raise RuntimeError

    return config_selection_learn, config_selection_predict, nfp_learn_values, nfp_predict_values
