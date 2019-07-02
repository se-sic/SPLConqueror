# Note: Since git repositories are cloned, an active internet connection is required

# The predictions were performed on Debian 9 (stretch)
FROM debian:stretch

# Set the working directory to /app
WORKDIR /application

# Set up specific apt package repositories (if needed)
RUN apt update

# Install git and wget
RUN apt install -y -qq git wget unzip mono-complete

# Download z3 (the library is needed for the constraint solver that is used inside SPL Conqueror)
RUN wget https://github.com/Z3Prover/z3/releases/download/z3-4.7.1/z3-4.7.1-x64-debian-8.10.zip \
    && unzip z3-4.7.1-x64-debian-8.10.zip \
    && rm z3-4.7.1-x64-debian-8.10.zip \
    && mv z3-4.7.1-x64-debian-8.10 z3 \
    && cp z3/bin/libz3.so /usr/lib/libz3.so

# Download SPL Conqueror and build it
RUN git clone --depth=1 https://github.com/se-passau/SPLConqueror.git \
    && cd SPLConqueror/SPLConqueror/ \
    && git submodule update --init \
    && wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe \
    && mono nuget.exe restore ./ -MSBuildPath /usr/lib/mono/xbuild/14.0/bin \
    && xbuild /p:Configuration=Release /p:TargetFrameworkVersion="v4.5" /p:TargetFrameworkProfile="" ./SPLConqueror.sln \
    && cd ../..

# Install Python and its dependencies for the ML algorithms
RUN apt install -y -qq python3 virtualenv \
    && virtualenv --python=python3 python3-env \
    && . ./python3-env/bin/activate \
    && pip3 install scikit-learn==0.19

