#!/bin/bash

DOXYGEN=/Applications/Doxygen.app/Contents/Resources/doxygen

function generate() {
	name="$1"
	framework="$2"

	# Check for directory presence
    if [ -d $framework ]; then
    	mkdir -p Work
    	cp $framework/*.h Work/
    	
    	# Check for script presence
    	if [ -f "$name.sh" ]; then
    		"./$name.sh"
    	fi
    
    	# Prepare the configuration
        cat doxygen.cfg | sed -e "s|@FRAMEWORK@|Work|g" > $name.cfg
        
        # Launch Doxygen
        $DOXYGEN $name.cfg
    
    	# Prepare dirs
        rm -Rf $name
        rm -Rf Input
        mv xml Input
	
		# Run the fragment generator
        mono Monobjc.Doxygenator.exe
	
        mv Output $name
        rm $name.cfg
        rm -Rf Input
        
        rm -Rf Work
    else
        echo "framework '$name' not found. Skipping."
    fi
}

#
# Don't go on if Doxygen is missing
#
if [ ! -f $DOXYGEN ]; then
    echo "'doxygen' command not found. Skipping."
    exit 0;
fi

#
# Generate the first-pass
#
generate "WebKit.DOM"		"/System/Library/Frameworks/WebKit.framework/Headers"
generate "CorePlot"			"/Library/Frameworks/CorePlot.framework/Headers"
generate "Growl"	 		"/Library/Frameworks/Growl.framework/Headers"
generate "SM2DGraphView"	"/Library/Frameworks/SM2DGraphView.framework/Headers"
generate "Sparkle"			"/Library/Frameworks/Sparkle.framework/Headers"

#
# Re-order files
#
if [ -d "CorePlot" ]; then
    cd "CorePlot"
    cd -
fi

if [ -d "Growl" ]; then
    cd "Growl"
    mv "C/NSObject_GrowlApplicationBridgeDelegate_InformalProtocol"					"P/GrowlApplicationBridgeDelegate"
    mv "C/NSObject_GrowlApplicationBridgeDelegate_Installation_InformalProtocol"	"P/GrowlApplicationBridgeInstallationDelegate"
    cd -
fi

if [ -d "SM2DGraphView" ]; then
    cd "SM2DGraphView"
    mv "C/NSObject_SM2DGraphDataSource"		"P/SM2DGraphDataSource"
    mv "C/NSObject_SM2DGraphDelegate"		"P/SM2DGraphDelegate"
    mv "C/NSObject_SMPieChartDataSource"	"P/SMPieChartDataSource"
    mv "C/NSObject_SMPieChartDelegate"		"P/SMPieChartDelegate"
    cd -
fi

if [ -d "Sparkle" ]; then
    cd "Sparkle"
    mv "C/NSObject(SUAppcastDelegate)"					"P/SUAppcastDelegate"
    mv "C/NSObject(SUUpdaterDelegateInformalProtocol)"	"P/SUUpdaterDelegateInformalProtocol"
    cd -
fi
