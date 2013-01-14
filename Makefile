## =======================================
##  __  __                   _     _
## |  \/  | ___  _ __   ___ | |__ (_) ___ 
## | |\/| |/ _ \| '_ \ / _ \| '_ \| |/ __|
## | |  | | (_) | | | | (_) | |_) | | (__ 
## |_|  |_|\___/|_| |_|\___/|_.__// |\___|
##                              |__/
## Top level makefile
## =======================================

# ----------------------------------------
# Variables
# ----------------------------------------

# Set the directories
export BUILD_DIR?=$(CURDIR)/build
export DIST_DIR?=$(CURDIR)/dist

# Set the files
export KEY_FILE?=$(CURDIR)/Monobjc.snk

# Compute the version
export MONOBJC_VERSION?=5.0

# Set the tools
export CPC?=rsync -a
export MKDIR?=mkdir -p
export RMRF?=rm -Rf
export XBUILD?=xbuild /p:Configuration=Release /verbosity:minimal

PROJECTS= \
	Monobjc.Tools \
	Monobjc.NAnt \
	Monobjc.MSBuild \
	Monobjc.Sdp

GENERATOR=Monobjc.Generator.NAnt
MARKER_FILE=.generated
GENERATOR_FILES= \
	$(wildcard $(GENERATOR)/bin/Release/Files/**/*.xml) \
	$(wildcard $(GENERATOR)/bin/Release/Resources/*.xml) \
	$(wildcard $(GENERATOR)/bin/Release/Resources/**/*.xml)

# ----------------------------------------
# Targets
# ----------------------------------------

all:
	$(MKDIR) "$(BUILD_DIR)"
	$(MKDIR) "$(DIST_DIR)"
	for i in $(PROJECTS); do \
		(mkdir -p $(BUILD_DIR)/$$i; $(XBUILD) /p:OutDir=$(BUILD_DIR)/$$i/ $$i/$$i.csproj); \
	done;
	$(CPC) "$(BUILD_DIR)/Monobjc.Tools/Monobjc.Tools.dll" "$(DIST_DIR)"
	$(CPC) "$(BUILD_DIR)/Monobjc.NAnt/Monobjc.NAnt.dll" "$(DIST_DIR)"
	$(CPC) "$(BUILD_DIR)/Monobjc.MSBuild/Monobjc.MSBuild.dll" "$(DIST_DIR)"
	$(CPC) "$(BUILD_DIR)/Monobjc.Sdp/Monobjc.Tools.Sdp.exe" "$(DIST_DIR)"

clean:
	for i in $(PROJECTS); do \
		($(XBUILD) /p:OutDir=$(BUILD_DIR)/$$i/ $$i/$$i.csproj /t:Clean); \
	done;
	$(XBUILD) $(GENERATOR)/$(GENERATOR).csproj /t:Clean
	$(RMRF) "$(BUILD_DIR)"
	$(RMRF) "$(DIST_DIR)"

generate-doc:

generate-wrappers: $(MARKER_FILE)

$(MARKER_FILE): $(GENERATOR_FILES)
	$(XBUILD) $(GENERATOR)/$(GENERATOR).csproj
	(cd $(GENERATOR)/bin/Release && mono NAnt.exe copy-in-place);
	if [ ! -f $(MARKER_FILE) ]; then touch $(MARKER_FILE); fi;
