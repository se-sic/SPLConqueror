import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
import sklearn.model_selection as modelSel
import sklearn.linear_model as sklm
from xgboost import XGBRegressor

import numpy as np

# default parameter spaces used for parameter tuning.

param_SVR = {'C': [0.5, 1, 2, 5], 'epsilon': [0.3, 0.2, 0.1], 'gamma': ['auto'], 'coef0': [0, 1, 2],
             'shrinking': [True, False], 'tol': [5e-2, 1e-1, 2e-1, 5e-1]}

# Do we really want to parameter optimize the random state?
param_DecisionTree = {'splitter': ['best', 'random'], 'max_features': ['auto', 'sqrt', 'log2'],
                      'min_samples_leaf': [1, 2],
                      'random_state': [1, 2, 3]}

param_RandomForest = {'n_estimators': [10, 12, 15, 18, 20], 'max_features': ['auto', 'sqrt', 'log2'],
                      'random_state': [1, 2, 3]}

param_kNNRegressor = {'n_neighbors': [8, 9, 10, 11, 12, 13, 14, 15], 'weights': ['uniform', 'distance'],
                      'algorithm': ['auto'],
                      'p': [1, 2, 3]}

param_kernelRidge = {'alpha': [1e-6], 'kernel': ['rbf', 'poly', 'linear'],
                     'degree': [1, 2, 3], 'gamma': [0.01, 0.05, 0.1, 0.2]}

param_baggingSVR = {'n_estimators': [5, 8, 10, 12, 15], 'max_samples': [0.75, 0.875, 1],
                    'max_features': [0.5, 0.625, 0.75, 0.875, 1],
                    'bootstrap': [True, False], 'bootstrap_features': [True, False], 'random_state': [1, 2, 3],
                    'base_estimator__C': [0.1, 0.2, 0.5, 1, 2, 5, 10],
                    'base_estimator__epsilon': [0.5, 0.3, 0.2, 0.1, 0.01],
                    'base_estimator__kernel': ['rbf', 'linear', 'poly', 'sigmoid'],
                    'base_estimator__degree': [1, 2, 3, 4, 5],
                    'base_estimator__coef0': [0, 1, 2, 3], 'base_estimator__shrinking': [True, False],
                    'base_estimator__tol': [1e-3, 2e-3, 5e-3, 1e-2, 2e-2, 5e-2, 1e-1]}

param_elastic_net = {"alpha": [0.25, 0.5, 0.75, 1.0],
                     "l1_ratio": [0, 0.25, 0.5, 0.75, 1.0],
                     "fit_intercept": [True, False],
                     "precompute": [False, True],
                     "max_iter": [100, 500, 750, 1000, 1250],
                     "copy_X": [True, bool],
                     "tol": [1e-5, 1e-4, 1e-3, 1e-2],
                     "warm_start": [True, False],
                     "positive": [False, False],
                     "random_state": [None, int],
                     "selection": ['cyclic', str]}

param_xgboost = {"n_extimators": [100, int],
                 "max_depth": [None, int],
                 "learning_rate": [None, float],
                 "booster": [None, str],
                 "tree_method": [None, str],
                 "n_jobs": [None, int],
                 "gamma": [None, float],
                 "min_child_weight": [None, float],
                 "max_delta_step": [None, float],
                 "subsample": [None, float],
                 "colsample_bytree": [None, float],
                 "colsample_bylevel": [None, float],
                 "colsample_bynode": [None, float],
                 "reg_alpha": [None, float],
                 "reg_lambda": [None, float],
                 "scale_pos_weight": [None, float],
                 "base_score": [None, float],
                 "random_state": [None, int],
                 "num_parallel_tree":  [None, int],
                 "importance_type": [None, str],
                 "validate_parameters": [True, bool]}

target_path = ""

strat_filename = "strat.txt"

output_filename = "Output.txt"


def setOutputPath(path):
    global target_path
    target_path = path


# Perform parameter tuning.
def optimizeParameter(strategy, X_train, y_train, parameter_space_def):
    strategy = strategy.lower()

    change_parameter_space(strategy, parameter_space_def)

    if strategy == "svr":
        return optimize_SVR(X_train, y_train)
    if strategy == "decisiontreeregression":
        return optimize_DecisionTree(X_train, y_train)
    elif strategy == "randomforestregressor":
        return optimize_RandomForestRegressor(X_train, y_train)
    elif strategy == "kneighborsregressor":
        return optimize_KNNeighborsRegressor(X_train, y_train)
    elif strategy == "kernelridge":
        return optimize_KernelRidge(X_train, y_train)
    elif strategy == "baggingsvr":
        return optimize_BaggingSVR(X_train, y_train)
    elif strategy == "xgboost":
        return optimize_xgboost(X_train, y_train)
    elif strategy == "elasticnet":
        return optimize_elastic_net(X_train, y_train)


# Score function used measure how good the configurations/estimator is
def scoreFunction(estimator, configurations, measurements):
    predictions = estimator.predict(configurations)
    sum = 0.0
    for i in range(len(measurements)):
        sum = np.abs(measurements[i] - predictions[i]) / measurements[i]

    return sum * -1


def optimize_SVR(X_train, y_train):
    opt = modelSel.RandomizedSearchCV(estimator=sk.SVR(cache_size=4000), param_distributions=param_SVR, cv=5,
                                      scoring=scoreFunction)
    opt.fit(X_train, y_train)

    return formatOptimal(opt.best_params_)


def optimize_DecisionTree(X_train, y_train):
    opt = modelSel.RandomizedSearchCV(estimator=skTr.DecisionTreeRegressor(), param_distributions=param_DecisionTree,
                                      cv=5, scoring=scoreFunction)
    opt.fit(X_train, y_train)
    return formatOptimal(opt.best_params_)


def optimize_RandomForestRegressor(X_train, y_train):
    opt = modelSel.GridSearchCV(estimator=skEn.RandomForestRegressor(), param_grid=param_RandomForest,
                                      cv=5, scoring=scoreFunction)
    opt.fit(X_train, y_train)
    return formatOptimal(opt.best_params_)


def optimize_KNNeighborsRegressor(X_train, y_train):
    opt = modelSel.RandomizedSearchCV(estimator=skNE.KNeighborsRegressor(), param_distributions=param_kNNRegressor,
                                      cv=5, scoring=scoreFunction)
    opt.fit(X_train, y_train)
    return formatOptimal(opt.best_params_)


def optimize_KernelRidge(X_train, y_train):
    opt = modelSel.RandomizedSearchCV(estimator=skKR.KernelRidge(), param_distributions=param_kernelRidge,
                                      cv=5, scoring=scoreFunction)
    opt.fit(X_train, y_train)
    return formatOptimal(opt.best_params_)


def optimize_BaggingSVR(X_train, y_train):
    opt = modelSel.RandomizedSearchCV(
        estimator=skEn.BaggingRegressor(base_estimator=sk.SVR(cache_size=500, gamma='auto'))
        , param_distributions=param_baggingSVR, cv=5, scoring=scoreFunction)
    opt.fit(X_train, y_train)

    return formatOptimal(opt.best_params_)

def optimize_elastic_net(x_train, y_train):
    opt = modelSel.RandomizedSearchCV(estimator=sklm.ElasticNet(), param_distributions=param_elastic_net,
                                      cv=5, scoring=scoreFunction)
    opt.fit(x_train, y_train)
    return formatOptimal(opt.best_params_)

def optimize_xgboost(x_train, y_train):
    pass

# Format the best configuration found during parameter tuning.
def formatOptimal(optimalParams):
    if len(optimalParams.keys()) == 0:
        return "No parameter has an influence on the accuracy of the result!"
    output = ""
    for key in optimalParams:
        output += key + "=" + str(optimalParams[key]) + ";"
    return output


# Format a user defined parameter space to one that can be eecuted by python.
def format_parameter_space(parameter_space):
    formated_space = ""
    for parameter in parameter_space:
        if ":" in parameter:
            name_value_pair = parameter.split(":")
            formated_space += " '" + name_value_pair[0] + "' : "
            formated_space += str(name_value_pair[1]).replace(';', ',') + ","
    formated_space += formated_space[:-1]
    formated_space += "}]"
    return formated_space


# Change the default parameter space to user given one.
def change_parameter_space(strategy, parameter_space_def):
    if parameter_space_def:
        to_execute = ""
        if strategy == "svr":
            to_execute = "param_SVR"
        elif strategy == "decisiontreeregression":
            to_execute = "param_DecisionTree"
        elif strategy == "randomforestregressor":
            to_execute = "param_RandomForest"
        elif strategy == "kneighborsregressor":
            to_execute = "param_kNNRegressor"
        elif strategy == "kernelridge":
            to_execute = "param_kernelRidge"
        elif strategy == "baggingsvr":
            to_execute = "param_baggingSVR"
        elif strategy == "xgboost":
            to_execute = "param_xgboost"
        elif strategy == "elasticnet":
            to_execute = "param_elastic_net"
        to_execute += " = [{"
        to_execute += format_parameter_space(parameter_space_def)
        exec(to_execute, globals())
