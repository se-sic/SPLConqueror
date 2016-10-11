import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
import sklearn.grid_search as modelSel

import numpy as np

param_SVR = [
  {'C': [0.5, 1, 2,5,10], 'epsilon': [0.3, 0.2, 0.1, 0.01], 'kernel': ['rbf', 'linear', 'poly', 'sigmoid'], 'degree' : [1, 2, 3, 4] , 'coef0' : [0,1,2], 'shrinking' : [True, False], 'tol' : [5e-3, 1e-2, 2e-2, 5e-2, 1e-1]} 
  ]

param_DecisionTree = [
   { 'splitter' : ['best', 'random'], 'max_features' : ['auto','sqrt','log2'], 'min_samples_leaf' : [1, 2], 'random_state' : [1, 2, 3] }
  ]

param_RandomForest = [
   { 'n_estimators' : [10, 12, 15, 18, 20] , 'max_features' : ['auto','sqrt','log2'], 'random_state' : [1, 2, 3]}
  ]

param_kNNRegressor = [
   { 'n_neighbors' : [8,9,10,11,12,13,14,15], 'weights' : ['uniform', 'distance'], 'algorithm' : ['auto'], 'p' : [1,2,3], 'n_jobs' : [-1]} 
   ]

param_kernelRidge = [
 {'alpha' : [1e0, 0.1, 1e-2, 1e-3, 1e-4, 1e-5], 'kernel' : ['rbf', 'linear', 'poly', 'sigmoid'], 'degree' : [1,2,3,4,5], 'gamma' : [0.01, 0.05, 0.1, 0.2, 0.3]}
  ]

param_baggingSVR = [
  {'n_estimators' : [5, 8, 10, 12, 15], 'max_samples' : [0.75,0.875,1], 'max_features' : [0.5,0.625,0.75,0.875,1] , 'bootstrap' : [True, False], 'bootstrap_features' : [True, False], 'random_state' : [1, 2, 3],  'base_estimator__C': [0.1, 0.2, 0.5, 1, 2,5,10], 'base_estimator__epsilon': [0.5, 0.3, 0.2, 0.1, 0.01], 'base_estimator__kernel': ['rbf', 'linear', 'poly', 'sigmoid'], 'base_estimator__degree' : [1, 2, 3, 4, 5] , 'base_estimator__coef0' : [0,1,2,3], 'base_estimator__shrinking' : [True, False], 'base_estimator__tol' : [1e-3, 2e-3, 5e-3, 1e-2, 2e-2, 5e-2, 1e-1]}
  
  ]

def optimizeParameter(strategy,X_train, y_train): 
  strategy = strategy.lower()
  
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


def scoreFunction(estimator, configurations, measurements):
  predictions = estimator.predict(configurations)
  sum = 0.0
  for i in range(len(measurements)): 
    sum = np.abs(measurements[i] - predictions[i]) / measurements[i]
  return sum


def optimize_SVR(X_train, y_train):
  opt = modelSel.GridSearchCV(sk.SVR(), param_SVR, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  return formatOptimal(opt.best_params_)

def optimize_DecisionTree(X_train, y_train):
  opt = modelSel.GridSearchCV(skTr.DecisionTreeRegressor(), param_DecisionTree, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  return formatOptimal(opt.best_params_)

def optimize_RandomForestRegressor(X_train, y_train):
  opt = modelSel.GridSearchCV(skEn.RandomForestRegressor(), param_RandomForest, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  return formatOptimal(opt.best_params_)

def optimize_KNNeighborsRegressor(X_train, y_train):
  opt = modelSel.GridSearchCV(skNE.KNeighborsRegressor(), param_kNNRegressor, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  return formatOptimal(opt.best_params_)

def optimize_KernelRidge(X_train, y_train):
  opt = modelSel.GridSearchCV(skKR.KernelRidge(), param_kernelRidge, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  return formatOptimal(opt.best_params_)

def optimize_BaggingSVR(X_train, y_train):
  opt = modelSel.GridSearchCV(skEn.BaggingRegressor(base_estimator=sk.SVR()), param_baggingSVR, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  
  return formatOptimal(opt.best_params_)


def formatOptimal(optimalParams): 
  if len(optimalParams.keys()) == 0: 
     return "No parameter has an influence on the accuracy of the result!"  
  output = ""
  for key in optimalParams:
    output += key +"="+str(optimalParams[key])+";"
  return output    