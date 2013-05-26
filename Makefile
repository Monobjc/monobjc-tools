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
export EXTERNAL_DIR=$(CURDIR)/external
export CONFIGURATION=Release

# Set the files
export KEY_FILE?=$(CURDIR)/Monobjc.snk

# Compute the version
export MONOBJC_VERSION?=5.0

# Set the tools
export CPC?=rsync -a
export MCS=dmcs -sdk:4 -nowarn:"219,1574,1584,1591"
export MKDIR?=mkdir -p
export RESGEN=resgen
export RMRF?=rm -Rf
export XBUILD?=xbuild /p:Configuration=$(CONFIGURATION) /verbosity:minimal

PROJECTS= \
	Monobjc.Tools \
	Monobjc.NAnt \
	Monobjc.MSBuild \
	Monobjc.Sdp

GENERATOR=Monobjc.Generator.NAnt
MARKER_FILE=.generated
GENERATOR_FILES= \
	$(wildcard $(GENERATOR)/bin/$(CONFIGURATION)/Files/**/*.xml) \
	$(wildcard $(GENERATOR)/bin/$(CONFIGURATION)/Resources/*.xml) \
	$(wildcard $(GENERATOR)/bin/$(CONFIGURATION)/Resources/**/*.xml)

# ----------------------------------------
# Targets
# ----------------------------------------

all:
	$(MKDIR) "$(BUILD_DIR)"
	$(MKDIR) "$(DIST_DIR)"
	for i in $(PROJECTS); do \
		($(MAKE) -C $$i all); \
	done;
	$(RMRF) $(DIST_DIR)/*.mdb

clean:
	for i in $(PROJECTS); do \
		($(MAKE) -C $$i clean); \
	done;
	$(XBUILD) $(GENERATOR)/$(GENERATOR).csproj /t:Clean
	$(RMRF) "$(BUILD_DIR)"
	$(RMRF) "$(DIST_DIR)"

generate-doc:

generate-wrappers: $(MARKER_FILE)

$(MARKER_FILE): $(GENERATOR_FILES)
	$(XBUILD) $(GENERATOR)/$(GENERATOR).csproj
	(cd $(GENERATOR)/bin/$(CONFIGURATION) && mono NAnt.exe copy-in-place);
	if [ ! -f $(MARKER_FILE) ]; then touch $(MARKER_FILE); fi;
