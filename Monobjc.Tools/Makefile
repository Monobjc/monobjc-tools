## =======================================
## Common Library Makefile
## =======================================

# ----------------------------------------
# Variables
# ----------------------------------------

NAME=Monobjc.Tools
TARGET=$(DIST_DIR)/$(NAME).dll
REFERENCES=-r:System.Security -r:ICSharpCode.SharpZipLib
SOURCES= \
	$(wildcard *.cs) \
	$(wildcard */*.cs) \
	$(wildcard */*/*.cs)

RESX=$(wildcard Properties/*.resx)
RESOURCES=$(patsubst %.resx,%.resources,$(RESX))
RESOURCES_ARGUMENT=$(foreach r,$(RESOURCES),-resource:$r,$(NAME).Properties.$(notdir $r))

# ----------------------------------------
# Targets
# ----------------------------------------

all: $(TARGET)

clean:

$(RESOURCES): $(RESX)
	$(RESGEN) $(RESX) $(RESOURCES)

$(TARGET): $(SOURCES) $(RESOURCES)
	$(MCS) -keyfile:$(KEY_FILE) -target:library -out:"$(TARGET)" $(REFERENCES) $(RESOURCES_ARGUMENT) $(SOURCES)

# ----------------------------------------
# Phony Targets
# ----------------------------------------

.PHONY: \
	all \
	clean
