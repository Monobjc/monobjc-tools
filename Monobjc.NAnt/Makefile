## =======================================
## Common Library Makefile
## =======================================

# ----------------------------------------
# Variables
# ----------------------------------------

NAME=Monobjc.NAnt
TARGET=$(DIST_DIR)/$(NAME).dll
REFERENCES=-r:System.Security -r:System.Xml -r:ICSharpCode.SharpZipLib -r:$(EXTERNAL_DIR)/NAnt.Core.dll -r:$(EXTERNAL_DIR)/NAnt.DotNetTasks.dll
SOURCES= \
	$(wildcard ../Monobjc.Tools/*.cs) \
	$(wildcard ../Monobjc.Tools/External/*.cs) \
	$(wildcard ../Monobjc.Tools/Generators/*.cs) \
	$(wildcard ../Monobjc.Tools/InterfaceBuilder/*.cs) \
	$(wildcard ../Monobjc.Tools/InterfaceBuilder/Visitors/*.cs) \
	$(wildcard ../Monobjc.Tools/ObjectiveC/*.cs) \
	$(wildcard ../Monobjc.Tools/Properties/Resources.Designer.cs) \
	$(wildcard ../Monobjc.Tools/PropertyList/*.cs) \
	$(wildcard ../Monobjc.Tools/Utilities/*.cs) \
	$(wildcard ../Monobjc.Tools/Xcode/*.cs) \
	$(wildcard ../Monobjc.Tools/Xcode/Visitors/*.cs) \
	$(wildcard *.cs) \
	$(wildcard */*.cs)

RESX=../Monobjc.Tools/Properties/Resources.resx Properties/Resources.resx
RESOURCES=$(patsubst %.resx,%.resources,$(RESX))
RESOURCES_ARGUMENT=-resource:../Monobjc.Tools/Properties/Resources.resources,Monobjc.Tools.Properties.Resources.resources -resource:Properties/Resources.resources,Monobjc.MSBuild.Properties.Resources.resources

# ----------------------------------------
# Targets
# ----------------------------------------

all: $(TARGET)

clean:

$(RESOURCES): $(RESX)
	$(RESGEN) $(RESX) $(RESOURCES)

$(TARGET): $(SOURCES) $(RESOURCES)
	$(MCS) -target:library -out:"$(TARGET)" $(REFERENCES) $(RESOURCES_ARGUMENT) $(SOURCES)

# ----------------------------------------
# Phony Targets
# ----------------------------------------

.PHONY: \
	all \
	clean
