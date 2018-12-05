"""Small script to install all needed Packages required to run the python functionalities.
Note: pip is required in order to install packages, it usually is distributed with all python versions >=2.7.9
Note: if you already have supported scikit-learn, numpy and scipy packages installed this
script isnt required.
Note:Alternatively you can also install all needed packages manually or use Anaconda
Note:Internet connection may be required."""
class MissingPipError(ImportError):
    def __init__(self, value):
        self.value = value
    def __str__(self):
        return repr(self.value)

"""Name and version of the scikit-learn package.
It is advised to use this version, as some needed
functions will be removed in sklearn >= 0.20
"""
sklearn_version = "scikit-learn==0.19.0"

"""numpy package needed for sklearn"""
numpy_version = "numpy==1.11.1"

"""scipy package needed for sklearn"""
scipy_version = "scipy==0.17.1"

try:
    import pip
    pip._internal.main(['install', numpy_version])
    pip._internal.main(['install', scipy_version])
    pip._internal.main(['install', sklearn_version])
except ImportError:
    raise MissingPipError("Pip has to be installed. Please install pip"
                          " Note pip is distributed with python version >= 2.7.9")
