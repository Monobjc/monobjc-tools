#!/bin/bash

DOXYGEN=/Applications/Doxygen.app/Contents/Resources/doxygen

function generate() {
	name="$1"

    framework="/Library/Frameworks/$name.framework/Headers"
    if [ -d $framework ]; then
        cat doxygen.cfg | sed -e "s/@FRAMEWORK@/$name/g" > $name.cfg
        $DOXYGEN $name.cfg
    
        rm -Rf $name
        rm -Rf Input
        mv xml Input
	
        mono Monobjc.Doxygenator.exe
	
        mv Output $name
        rm $name.cfg
        rm -Rf Input
    else
        echo "framework '$name' not found. Skipping."
    fi
}

if [ ! -f $DOXYGEN ]; then
    echo "'doxygen' command not found. Skipping."
    exit 0;
fi

#
# Generate the first-pass
#
generate "CorePlot"
generate "Growl"
generate "SM2DGraphView"
generate "Sparkle"

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
