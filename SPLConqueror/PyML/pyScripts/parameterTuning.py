import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
import sklearn.grid_search as modelSel

import numpy as np

param_SVR = [
 # {'C': [1, 10, 100, 1000], 'gamma': [0.001, 0.0001], 'kernel': ['rbf','linear']}
  {'C': [1, 10], 'gamma': [0.001], 'kernel': ['rbf']}
  ]

def optimizeParameter(strategy,X_train, y_train): 
  text_file = open("E:\SPLConquerorPython\Input.txt", "w")
  text_file.write(str(X_train))
  text_file.write("y_train")
  text_file.write(str(y_train))
  strategy = strategy.lower()
  if strategy == "svr":
    return optimize_SVR(X_train, y_train)
'''  elif strategy == "decisiontreeregressor":
    return optimize_DecisionTree()
  elif strategy == "randomforestregressor":
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
  opt = modelSel.GridSearchCV(sk.SVR(C =1), param_SVR, cv=5,scoring=scoreFunction)
  opt.fit(X_train, y_train)
  return opt.best_params_
  
  