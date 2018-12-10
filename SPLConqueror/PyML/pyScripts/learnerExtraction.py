from sklearn.tree import DecisionTreeRegressor
from sklearn.ensemble.forest import RandomForestRegressor
from sklearn.svm import SVR

paths = []
forest_paths = []

def extract_svr(svm):
    to_return = ""
    if svm.kernel == "linear":
        to_return += str(svm.intercept_[0])
        for (index, value) in enumerate(svm.coef_[0]):
            to_return += " + C(" + str(index) + ") * " + str(value)
    elif svm.kernel == "poly" and svm.gamma is not None and not svm._pairwise:
        #TODO Work in progress if needed
        raise NotImplemented
        to_return += str(svm.intercept_[0])
        for (index, value) in enumerate(svm.dual_coef_[0]):
            # TODO right indexing
            to_return += " + " + str(value) + "* (" + str(svm._gamma) + " * " + str(svm.support_vectors_[index][0]) + " * " + str(rep[0])  +")**" + str(svm.degree)
    else:
        raise NotImplemented
    return to_return


def extract_tree(tree):
    global paths
    paths = []
    if tree is None or tree.tree_.children_left is None:
        return ""
    recursion(tree.tree_.children_left, tree.tree_.children_right, tree.tree_.threshold, tree.tree_.feature, tree.tree_.value, 0, "")
    return paths


def extract_forest(forest):
    global paths
    global forest_paths
    for tree in forest.estimators_:
        paths = []
        if tree is None or tree.tree_.children_left is None:
            break
        recursion(tree.tree_.children_left, tree.tree_.children_right, tree.tree_.threshold, tree.tree_.feature,
                  tree.tree_.value, 0, "")
        forest_paths.append(paths)
    return forest_paths


def extract(learner):
    if isinstance(learner, DecisionTreeRegressor):
        return [extract_tree(learner)]
    elif isinstance(learner, RandomForestRegressor):
        return extract_forest(learner)
    elif isinstance(learner, SVR):
        return [extract_svr(learner)]
    else:
        raise NotImplementedError


def recursion(left, right, threshold, features, value, node, previous):
    # magic number for deciscion does not exist
    curr = ""
    if threshold[node] != -2:
        curr = "I(" + str(features[node]) + "<=" + str(threshold[node]) + ")"
        if previous is not "":
            curr = previous + "*" + curr
        if left[node] != -1:
            recursion(left, right, threshold, features, value, left[node], curr)
        if right[node] != -1:
            curr = previous + "I(" + str(features[node]) + ">" + str(threshold[node]) + ")"
            recursion(left, right, threshold, features, value, right[node], curr)
    else:
        curr += previous + "*" + str(value[node][0][0])
        paths.append(curr)