import sklearn.svm as sk
import sklearn.ensemble as skEn
import sklearn.neighbors as skNE
import sklearn.kernel_ridge as skKR
import sklearn.tree as skTr
import numpy as np

def setup_learning(strategy, kernel_setting):
    if strategy == "SVR":
        if kernel_setting == "standard":
             return sk.SVR(C=1.0, cache_size=200, coef0=0.0, degree=3, kernel='linear', gamma='auto', max_iter=-1, shrinking=True, tol=0.001, verbose=False)
        else:
            return sk.SVR(kernel=kernel_setting)
    elif strategy == "DecisionTreeRegressor":
          return skTr.DecisionTreeRegressor(criterion='mse', splitter='best', max_depth=None, min_samples_split=2, min_samples_leaf=1, min_weight_fraction_leaf=0.0, max_features=None, random_state=None, max_leaf_nodes=None, presort=False)
   
    elif strategy == "RandomForestRegressor":
        return skEn.RandomForestRegressor(n_estimators=10, criterion='mse', min_samples_split=2, max_features='auto', bootstrap=False, n_jobs=-1, random_state=None)
    elif strategy == "BaggingSVR": 
        return skEn.BaggingRegressor(sk.SVR(C=1.0, cache_size=200, coef0=0.0, degree=3, gamma='auto', kernel='linear', max_iter=-1, shrinking=True, tol=0.001, verbose=False),max_samples=0.5, max_features=1.0)
    elif strategy == "KNeighborsRegressor":
        return skNE.KNeighborsRegressor(n_neighbors=5, weights='uniform', algorithm='auto', leaf_size=30, p=2, metric='minkowski', metric_params=None, n_jobs=-1)
    elif strategy == "KERNELRIDGE":
          return skKR.KernelRidge(alpha=1, kernel='linear', gamma=None, degree=3, coef0=1, kernel_params=None)	

def learn(strategy, kernel_setting, X, y):
    model = setup_learning(strategy, kernel_setting)
    model.fit(X, y)
    return model

def predict(strategy, model, X, y): 
        return model.predict(X)   