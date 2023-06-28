import sys
from typing import Dict, List, Any

import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
from sklearn.linear_model import LinearRegression
from lineartree import LinearTreeRegressor
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
    elif strategy == "lineardecisiontreeregression":
        try:
            to_return = setup_LinearCART(learner_settings)
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


def setup_SVR(learner_settings):
    parameter = {"kernel": ['rbf', str],  # linear, poly, rbf, sigmoid, precomputd
                 "degree": [3, int],
                 "gamma": ["auto"],  # other options: 'scale' or float
                 "coef0": [0.0, float],
                 "tol": [0.001, float],
                 "C": [1.0, float],
                 "epsilon": [0.1, float],
                 "shrinking": [True, bool],
                 "cache_size": [200, float],
                 "verbose": [False, bool],
                 "max_iter": [-1, int],
                 }
    parameter = read_in_settings(learner_settings, parameter)
    return sk.SVR(**parameter)


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
    parameter = {"criterion": ['squared_error', str],  # friedman_mse, absolute_error, poisson
                 "splitter": ['best', str],  # random
                 "max_depth": [None, int],
                 "min_samples_split": [2, int],
                 "min_samples_leaf": [1, float],
                 "min_weight_fraction_leaf": [0.0, float],
                 "max_features": ["auto"],  # other options are auto, sqrt, log2 or int
                 "random_state": [None, int],
                 "max_leaf_nodes": [None, int],
                 "min_impurity_decrease": [0.0, float],
                 "ccp_alpha": [0.0, float],
                 }
    parameter = read_in_settings(learner_settings, parameter)
    return skTr.DecisionTreeRegressor(**parameter)


def setup_RandomForestRegressor(learner_settings):
    parameter = {"n_estimators": [10, int],
                 "criterion": ["squared_error", str],  # Other options: absolute_error, poisson
                 "max_depth": [None, int],
                 "min_samples_split": [2, int],
                 "min_samples_leaf": [1, int],
                 "min_weight_fraction_leaf": [0.0, float],
                 "max_features": ["auto", int],  # auto, sqrt, log2 or int
                 "max_leaf_nodes": [None, int],
                 "bootstrap": [True, bool],
                 "oob_score": [False, bool],
                 "n_jobs": [None, int],
                 "random_state": [None, int],
                 "verbose": [0, int],
                 "warm_start": [False, bool],
                 "ccp_alpha": [0.0, float],
                 "max_samples": [None, int]
                 }

    parameter = read_in_settings(learner_settings, parameter)
    return skEn.RandomForestRegressor(**parameter)


def setup_BaggingSVR(learner_settings):
    parameter = {"n_estimators": [10, int],
                 "max_samples": [1.0, float],
                 "max_features": [1.0, float],
                 "bootstrap": [True, bool],
                 "bootstrap_features": [False, bool],
                 "oob_score": [False, bool],
                 "n_jobs": [None, int],
                 "random_state": [None, int],
                 "verbose": [0, int],
                 "warm_start": [False, bool],
                 }
    base_estimator = setup_SVR(learner_settings)
    parameter = read_in_settings(learner_settings, parameter)
    return skEn.BaggingRegressor(base_estimator=base_estimator, **parameter)


def setup_KNeighborsRegressor(learner_settings):
    parameter = {"n_neighbors": [5, int],
                 "weights": ['uniform', str],  # or distance
                 "algorithm": ['auto', str],  # auto, ball_tree, kd_tree, brute
                 "leaf_size": [30, int],
                 "p": [2, int],
                 "metric": ['minkowski', str],
                 "metric_params": [None],
                 "n_jobs": [None, int],
                 }

    parameter = read_in_settings(learner_settings, parameter)
    return skNE.KNeighborsRegressor(**parameter)


def setup_KernelRidge(learner_settings):
    parameter = {"alpha": [1.0, float],
                 "kernel": ['linear', str],  # or distance
                 "gamma": [None, float],  # auto, ball_tree, kd_tree, brute
                 "degree": [3, float],
                 "coef0": [1.0, float],
                 "kernel_params": [None],
                 }

    parameter = read_in_settings(learner_settings, parameter)
    return skKR.KernelRidge(**parameter)


def setup_LinearCART(learner_settings):
    parameter = {"criterion": ["mse", str],
                 "max_depth": [5, int],
                 "min_samples_split": [6, float],
                 "min_samples_leaf": [0.1, float],
                 "max_bins": [25, int],
                 "min_impurity_decrease": [0.0, float],
                 "n_jobs": [None, int],
                 }

    parameter = read_in_settings(learner_settings, parameter)
    return LinearTreeRegressor(base_estimator=LinearRegression(), **parameter)


def read_in_settings(settings: List[str], parameter_list: Dict[str, List]) -> Dict[str, Any]:
    for setting in settings:
        setting_value_pair = setting.split(":")
        if setting_value_pair[0] in parameter_list.keys():
            key = setting_value_pair[0]
            if len(parameter_list[key]) > 1:
                # Convert it to the datatype given in the tuple
                parameter_list[key][0] = parameter_list[key][1](setting_value_pair[1])
            else:
                parameter_list[key][0] = setting_value_pair[1]
        else:
            print(f"Parameter {setting_value_pair[0]} is unknown.\n", file=sys.stderr, flush=True)
    parameter_to_value_dict = {}
    for dict_key in parameter_list.keys():
        parameter_to_value_dict[dict_key] = parameter_list[dict_key][0]
    return parameter_to_value_dict
