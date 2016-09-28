import sklearn.svm as sk
import numpy as np

def setup_learning(strategy, kernel_setting):
    if strategy == "SVR":
        if kernel_setting == "standard":
            return sk.SVR()
        else:
            return sk.SVR(kernel=kernel_setting)
    elif strategy == "LinearSVR":
        if kernel_setting == "standard":
            return sk.LinearSVR()
        else:
            return sk.LinearSVR(kernel=kernel_setting)
    elif strategy == "NuSVR":
        if kernel_setting == "standard":
            return sk.NuSVR()
        else:
            return sk.NuSVR(kernel=kernel_setting)

def learn(strategy, kernel_setting, X, y):
    model = setup_learning(strategy, kernel_setting)
   # X = np.array(configurations)
   # y = np.array(nfp_values)
    model.fit(X, y)
    if strategy == "LinearSVR" or kernel_setting:
        to_Return = []
        for number in model.coef_:
            to_Return.append(number)
        return to_Return