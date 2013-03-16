## =======================================
## Common Library Makefile
## =======================================

# ----------------------------------------
# Variables
# ----------------------------------------

NAME=Monobjc.Sdp
TARGET=$(DIST_DIR)/Monobjc.Tools.Sdp.exe
REFERENCES=-r:System.Xml -r:System.Xml.Linq
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
	$(CPC) monobjc-sdp $(DIST_DIR)

clean:

$(RESOURCES): $(RESX)
	$(RESGEN) $(RESX) $(RESOURCES)

$(TARGET): $(SOURCES) $(RESOURCES)
	$(MCS) -keyfile:$(KEY_FILE) -target:exe -out:"$(TARGET)" $(REFERENCES) $(RESOURCES_ARGUMENT) $(SOURCES)

# ----------------------------------------
# Phony Targets
# ----------------------------------------

.PHONY: \
	all \
	clean
