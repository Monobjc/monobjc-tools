#!/bin/bash

FILE=$1
TAG=$2

cat $FILE | sed -e "s/<li class=\"forums\">\(.*\)<\/li>/\1/g" > $FILE.1
cat $FILE.1 | sed -e "s/>\( \)*/>/g" -e "s/\( \)*</</g" > $FILE.2
cat $FILE.2 | sed -e "s/#.*\">\(.*\)</\">\1</g" > $FILE.3
cat $FILE.3 | sed -e "s/<a href=\"\(.*\)\">\(.*\)<\/a>/<$TAG name=\"\2\"><File>\1<\/File><\/$TAG>/g" > $FILE

rm -f $FILE.1
rm -f $FILE.2
rm -f $FILE.3
rm -f $FILE.4
