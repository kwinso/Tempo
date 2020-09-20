# TODO: 
# 2. Build in /opt
# 3. Create symlink to the binary on /opt
#

OPT_DIR="/opt/Tempo"

# Preparing for build.
sudo mkdir -p $OPT_DIR/bin

# Building Project
sudo dotnet build ../Tempo/Tempo.csproj -o $OPT_DIR

# Copying files
sudo cp -r Assets/* $OPT_DIR 

sudo ln -fs $OPT_DIR/Tempo.dll "/usr/local/bin/tempo" 

sudo chmod +x "/usr/local/bin/tempo"