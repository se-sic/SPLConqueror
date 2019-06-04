import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
import ast

number_of_configurations = 0


# setup the learner with the right settings.
def setup_learning(strategy, learner_settings):
    strategy = strategy.lower()
    if strategy == "svr":
        try:
            to_return = setup_SVR(learner_settings)
            return to_return
        except ValueError:
            return val_err_inf(learner_settings)
        except TypeError:
            return typ_err_inf(learner_settings)
    elif strategy == "decisiontreeregression":
        try:
            to_return = setup_DecisionTree(learner_settings)
            return to_return
        except ValueError:
            return val_err_inf(learner_settings)
        except TypeError:
            return typ_err_inf(learner_settings)
    elif strategy == "randomforestregressor":
        try:
            to_return = setup_RandomForestRegressor(learner_settings)
            return to_return
        except ValueError:
            return val_err_inf(learner_settings)
        except TypeError:
            return typ_err_inf(learner_settings)
    elif strategy == "baggingsvr":
        try:
            to_return = setup_BaggingSVR(learner_settings)
            return to_return
        except ValueError:
            return val_err_inf(learner_settings)
        except TypeError:
            return typ_err_inf(learner_settings)
    elif strategy == "kneighborsregressor":
        try:
            to_return = setup_KNeighborsRegressor(learner_settings)
            return to_return
        except ValueError:
            return val_err_inf(learner_settings)
        except TypeError:
            return typ_err_inf(learner_settings)
    elif strategy == "kernelridge":
        try:
            to_return = setup_KernelRidge(learner_settings)
            return to_return
        except ValueError:
            return val_err_inf(learner_settings)
        except TypeError:
            return typ_err_inf(learner_settings)


def val_err_inf(settings):
    return settings.append(
        " These learner settings caused a ValueError. Very likely due to one argument being invalid.")


def typ_err_inf(settings):
    return settings.append(
        " These learner settings caused a ValueError. Very likely due to one argument being the wrong type.")


def attr_err_inf(settings, learner):
    return settings.append(
        learner + ". Learner type is not valid and predict cant be used on None type. Please check for typos.")


class Learner:
    # initialize the learner
    def __init__(self, strategy, learner_configurations):
        self.learning_model = setup_learning(strategy, learner_configurations)
        self.strategy = strategy
        self.learner_configurations = learner_configurations

    # use the features and nfp_values to train the model
    def learn(self, x, y):
        try:
            self.learning_model.fit(x, y)
        except ValueError:
            return
        except TypeError:
            return
        except AttributeError:
            return

    def predict(self, x):
        try:
            return self.learning_model.predict(x)
        # evil hack that catches all exception, but needed so that the C# program wont be stuck
        except Exception:
            to_return = []
            to_return.append("Error has ocured. Please check your settings.")
            to_return.append("Strategy: " + self.strategy)
            config = ""
            for conf in self.learner_configurations:
                config += conf
            to_return.append("Configuration: " + config)
            to_return.append("Learner: " + str(self.learning_model))
            to_return.append(
                "If the settings are right the learner may have not been fitted due to wrong input format.")
            return to_return

    def predict_and_compare(self, features, nfp_values):
        # predict the value for each configuration and present it in a:
        # '[configuration] nfpvalue;predictionvalue' format
        prediction_results = self.predict_values(features)
        nfp_and_prediction_values = []
        i = 0
        if len(nfp_values) == len(prediction_results):
            for prediction_result in prediction_results:
                nfp_and_prediction_values.append(str(nfp_values[i]) + ";" + str(prediction_result))
                i += 1
            return nfp_and_prediction_values
        else:
            # should never be reached
            return "invalid state"


def setup_SVR(learner_settings):
    # default settings
    kernel = 'rbf'
    degree = 3
    gamma = 'auto'
    coef0 = 0.0
    tol = 0.001
    C = 1.0
    epsilon = 0.1
    shrinking = True
    cache_size = 200
    verbose = False
    max_iter = -1

    # change default values.
    for additional_setting in learner_settings:
        # split identifier=value, so you can identify value and the variable
        setting_value_pair = additional_setting.split("=")
        if setting_value_pair[0] == "kernel":
            kernel = setting_value_pair[1]
        if setting_value_pair[0] == "degree":
            degree = int(setting_value_pair[1])
        if setting_value_pair[0] == "gamma":
            gamma = setting_value_pair[1]
        if setting_value_pair[0] == "coef0":
            coef0 = float(setting_value_pair[1])
        if setting_value_pair[0] == "tol":
            tol = float(setting_value_pair[1])
        if setting_value_pair[0] == "C":
            C = float(setting_value_pair[1])
        if setting_value_pair[0] == "epsilon":
            epsilon = float(setting_value_pair[1])
        if setting_value_pair[0] == "shrinking":
            shrinking = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "cache_size":
            cache_size = int(setting_value_pair[1])
        if setting_value_pair[0] == "verbose":
            if not setting_value_pair[1].isnumeric():
                verbose = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "max_iter":
            max_iter = int(setting_value_pair[1])

    return sk.SVR(C=C, cache_size=cache_size, epsilon=epsilon, coef0=coef0, degree=degree,
                  kernel=kernel, gamma=gamma, max_iter=max_iter, shrinking=shrinking, tol=tol, verbose=verbose)


def parse_to_int_float_bool_string(n):
    try:
        x = int(n)
    except ValueError:
        try:
            x = float(n)
        except ValueError:
            try:
                x = ast.literal_eval(n)
            except ValueError:
                x = n
    return x


def setup_DecisionTree(learner_settings):
    # default values
    criterion = 'mse'
    splitter = 'best'
    max_depth = None
    min_samples_split = 2
    min_samples_leaf = 1
    min_weight_fraction_leaf = 0.0
    max_features = None
    random_state = None
    max_leaf_nodes = None
    # min impurity split is only supported in versions >=0.18
    # min_impurity_split = 1e-07
    presort = False

    # change default values
    for additional_setting in learner_settings:
        # split identifier=value, so you can identify value and the variable
        setting_value_pair = additional_setting.split("=")
        if setting_value_pair[0] == "criterion":
            criterion = setting_value_pair[1]
        if setting_value_pair[0] == "splitter":
            splitter = setting_value_pair[1]
        if setting_value_pair[0] == "max_depth":
            max_depth = int(setting_value_pair[1])
        if setting_value_pair[0] == "min_samples_split":
            min_samples_split = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "min_samples_leaf":
            min_samples_leaf = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "min_weight_fraction_leaf":
            min_weight_fraction_leaf = float(setting_value_pair[1])
        if setting_value_pair[0] == "max_features":
            max_features = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "random_state":
            random_state = int(setting_value_pair[1])
        if setting_value_pair[0] == "max_leaf_nodes":
            max_leaf_nodes = int(setting_value_pair[1])
        # if setting_value_pair[0] == "min_impurity_split":
        #    min_impurity_split = float(setting_value_pair[1])
        if setting_value_pair[0] == "presort":
            presort = (setting_value_pair[1] == "True")

    return skTr.DecisionTreeRegressor(criterion=criterion, splitter=splitter, max_depth=max_depth,
                                      min_samples_split=min_samples_split,
                                      min_samples_leaf=min_samples_leaf,
                                      min_weight_fraction_leaf=min_weight_fraction_leaf, max_features=max_features,
                                      random_state=random_state, max_leaf_nodes=max_leaf_nodes,
                                      presort=presort)  # min_impurity_split=min_impurity_split,)


def setup_RandomForestRegressor(learner_settings):
    # default values
    n_estimators = 10
    criterion = 'mse'
    max_depth = None
    min_samples_split = 2
    min_samples_leaf = 1
    min_weight_fraction_leaf = 0.0
    max_features = 'auto'
    max_leaf_nodes = None
    # min impurity split is only supported in versions >=0.18
    # min_impurity_split = 1e-07
    bootstrap = True
    oob_score = False
    n_jobs = 1
    random_state = None
    verbose = 0
    warm_start = False

    # change default values
    for additional_setting in learner_settings:
        # split identifier=value, so you can identify value and the variable
        setting_value_pair = additional_setting.split("=")
        if setting_value_pair[0] == "n_estimators":
            n_estimators = int(setting_value_pair[1])
        if setting_value_pair[0] == "criterion":
            criterion = setting_value_pair[1]
        if setting_value_pair[0] == "max_depth":
            max_depth = int(setting_value_pair[1])
        if setting_value_pair[0] == "min_samples_split":
            min_samples_split = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "min_samples_leaf":
            min_samples_leaf = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "min_weight_fraction_leaf":
            min_weight_fraction_leaf = float(setting_value_pair[1])
        if setting_value_pair[0] == "max_features":
            max_features = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "max_leaf_nodes":
            max_leaf_nodes = int(setting_value_pair[1])
        # if setting_value_pair[0] == "min_impurity_split":
        #    min_impurity_split = float(setting_value_pair[1])
        if setting_value_pair[0] == "bootstrap":
            bootstrap = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "oob_score":
            oob_score = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "n_jobs":
            n_jobs = int(setting_value_pair[1])
        if setting_value_pair[0] == "random_state":
            random_state = int(setting_value_pair[1])
        if setting_value_pair[0] == "verbose":
            if setting_value_pair[1].isnumeric():
                verbose = int(setting_value_pair[1])
        if setting_value_pair[0] == "warm_start":
            warm_start = (setting_value_pair[1] == "True")

    return skEn.RandomForestRegressor(n_estimators=n_estimators, criterion=criterion,
                                      min_samples_split=min_samples_split,
                                      max_features=max_features, bootstrap=bootstrap, n_jobs=n_jobs,
                                      random_state=random_state,
                                      warm_start=warm_start, verbose=verbose, oob_score=oob_score,
                                      max_leaf_nodes=max_leaf_nodes, min_weight_fraction_leaf=min_weight_fraction_leaf,
                                      min_samples_leaf=min_samples_leaf,
                                      max_depth=max_depth)


def setup_BaggingSVR(learner_settings):
    # default values
    base_estimator = setup_SVR(learner_settings)
    n_estimators = 10
    max_samples = 1.0
    max_features = 1.0
    bootstrap = True
    bootstrap_features = False
    oob_score = False
    warm_start = False
    n_jobs = 1
    random_state = None
    verbose = 0

    # change default values
    for additional_setting in learner_settings:
        # split identifier=value, so you can identify value and the variable
        setting_value_pair = additional_setting.split("=")
        if setting_value_pair[0] == "verbose":
            if setting_value_pair[1].isnumeric():
                verbose = int(setting_value_pair[1])
        if setting_value_pair[0] == "random_state":
            random_state = int(setting_value_pair[1])
        if setting_value_pair[0] == "n_jobs":
            n_jobs = int(setting_value_pair[1])
        if setting_value_pair[0] == "warm_start":
            warm_start = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "oob_score":
            oob_score = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "bootstrap_features":
            bootstrap_features = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "bootstrap":
            bootstrap = (setting_value_pair[1] == "True")
        if setting_value_pair[0] == "max_features":
            max_features = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "max_samples":
            max_samples = parse_to_int_float_bool_string(setting_value_pair[1])
        if setting_value_pair[0] == "n_estimators":
            n_estimators = int(setting_value_pair[1])

    return skEn.BaggingRegressor(base_estimator=base_estimator, n_estimators=n_estimators, max_samples=max_samples,
                                 max_features=max_features, bootstrap=bootstrap,
                                 bootstrap_features=bootstrap_features, oob_score=oob_score, warm_start=warm_start,
                                 n_jobs=n_jobs, random_state=random_state, verbose=verbose)


def setup_KNeighborsRegressor(learner_settings):
    # default values
    n_neighbors = 5
    weights = 'uniform'
    algorithm = 'auto'
    leaf_size = 30
    p = 2
    metric = 'minkowski'
    metric_params = None
    n_jobs = 1

    # change default values
    for additional_setting in learner_settings:
        # split identifier=value, so you can identify value and the variable
        setting_value_pair = additional_setting.split("=")
        if setting_value_pair[0] == "n_neighbors":
            if int(setting_value_pair[1]) <= number_of_configurations:
                n_neighbors = int(setting_value_pair[1])
            else:
                n_neighbors = 5
        if setting_value_pair[0] == "weights":
            weights = setting_value_pair[1]
        if setting_value_pair[0] == "algorithm":
            algorithm = setting_value_pair[1]
        if setting_value_pair[0] == "leaf_size":
            leaf_size = int(setting_value_pair[1])
        if setting_value_pair[0] == "p":
            p = int(setting_value_pair[1])
        if setting_value_pair[0] == "metric":
            metric = setting_value_pair[1]
        if setting_value_pair[0] == "n_jobs":
            n_jobs = int(setting_value_pair[1])

    return skNE.KNeighborsRegressor(n_neighbors=n_neighbors, weights=weights, algorithm=algorithm,
                                    leaf_size=leaf_size, p=p, metric=metric, metric_params=metric_params,
                                    n_jobs=n_jobs)


def setup_KernelRidge(learner_settings):
    alpha = 1
    kernel = 'linear'
    gamma = None
    degree = 3
    coef0 = 1
    kernel_params = None

    for additional_setting in learner_settings:
        # split identifier=value, so you can identify value and the variable
        setting_value_pair = additional_setting.split("=")
        if setting_value_pair[0] == "alpha":
            alpha = float(setting_value_pair[1])
        if setting_value_pair[0] == "kernel":
            kernel = setting_value_pair[1]
        if setting_value_pair[0] == "gamma":
            gamma = float(setting_value_pair[1])
        if setting_value_pair[0] == "degree":
            degree = int(setting_value_pair[1])
        if setting_value_pair[0] == "coef0":
            coef0 = int(setting_value_pair[1])
        if setting_value_pair[0] == "kernel_params":
            kernel_params = setting_value_pair[1]

    return skKR.KernelRidge(alpha=alpha, kernel=kernel, gamma=gamma, degree=degree, coef0=coef0,
                            kernel_params=kernel_params)
