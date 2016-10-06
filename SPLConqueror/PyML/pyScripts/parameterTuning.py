import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
import sklearn.grid_search as modelSel

import numpy as np

param_SVR = [
 # {'C': [1, 10, 100, 1000], 'gamma': [0.001, 0.0001], 'kernel': ['rbf','linear']}
   {'C': [0.1, 0.2, 0.5, 1, 2], 'epsilon': [0.5, 0.3, 0.2, 0.1, 0.01], 'kernel': ['rbf', 'linear', 'poly', 'sigmoid'], 'degree' : [1, 2, 3, 4, 5] , 'coef0' : [0,1,2,3], 'shrinking' : [True, False], 'tol' : [1e-3, 2e-3, 5e-3, 1e-2]}
  #{'C': [0.1, 0.2], 'epsilon': [0.5, 0.3], 'kernel': ['poly', 'sigmoid'], 'degree' : [4, 5] , 'coef0' : [2,3], 'shrinking' : [True, False], 'tol' : [5e-3, 1e-2]}
  ]
param_DecisionTree = [
   { 'citerion' : ['mse','mae'], 'splitter' : ['best', 'random'], 'max_features' : ['auto','sqrt','log2'], 'min_samples_leaf' : [1, 2], 'random_state' : [1, 2, 3], 'min_impurity_split' : [1e-7, 1e-6, 1e-5, 1e-4] }
  ]

  
def optimizeParameter(strategy,X_train, y_train): 
  text_file = open("E:\SPLConquerorPython\Input.txt", "w")
  text_file.write(str(X_train))
  text_file.write("y_train")
  text_file.write(str(y_train))
  strategy = strategy.lower()
  if strategy == "svr":
    return optimize_SVR(X_train, y_train)
  elif strategy == "decisiontreeregressor":
    return optimize_DecisionTree()
'''  elif strategy == "randomforestregressor":
    return optimize_RandomForestRegressor()
  elif strategy == "baggingsvr": 
    return optimize_BaggingSVR()
  elif strategy == "knneighborsregressor":
    return optimizep_KNeighborsRegressor()
  elif strategy == "kernelridge":
    return optimize_KernelRidge()
'''

def scoreFunction(estimator, configurations, measurements):
  text_file = open("E:\SPLConquerorPython\Output.txt", "w")
  text_file.write(str(estimator))
  text_file.write("sum")
  predictions = estimator.predict(configurations)
  sum = 0.0
  for i in range(len(measurements)): 
    sum = np.abs(measurements[i] - predictions[i]) / measurements[i]
  text_file.write(str(sum))
  text_file.close()
  return sum
    
    
def optimize_SVR(X_train, y_train):
  opt = modelSel.GridSearchCV(sk.SVR(), param_SVR, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  output = ""
  for key in opt.best_params_:
    output += key +"="+str(opt.best_params_[key])+";"
  return output  
  
def optimize_DecisionTree(X_train, y_train):
  opt = modelSel.GridSearchCV(sk.DecisionTreeRegressor(), param_SVR, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  output = ""
  for key in opt.best_params_:
    output += key +"="+str(opt.best_params_[key])+";"
  return output    
  