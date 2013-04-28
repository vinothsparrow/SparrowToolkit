@echo off
cd ..
cd Build
IF EXIST 4.0 GOTO 4
mkdir 4.0
:4
IF EXIST 3.5 GOTO 3
mkdir 3.5
:3
IF EXIST Silverlight4 GOTO Silverlight4
mkdir Silverlight4
:Silverlight4
IF EXIST Silverlight5 GOTO Silverlight5
mkdir Silverlight5
:Silverlight5

cd ..
cd Work
copy  "Document\Help\*.chm" "..\Build\4.0"
copy  "Source\Sparrow.Chart\WPF\bin\Release\*.xml" "..\Build\4.0"
copy  "Source\Sparrow.Chart\WPF\bin\Release\*.dll" "..\Build\4.0"
copy  "Source\Sparrow.Chart\WPF\bin\Release\3.5\*.xml" "..\Build\3.5"
copy  "Source\Sparrow.Chart\WPF\bin\Release\3.5\*.dll" "..\Build\3.5"
copy "Source\Sparrow.Chart\Silverlight\Bin\Release\*.dll" "..\Build\Silverlight5"
copy "Source\Sparrow.Chart\Silverlight\Bin\Release\*.xml" "..\Build\Silverlight5"
copy "Source\Sparrow.Chart\Silverlight\Bin\Release\SL4\*.dll" "..\Build\Silverlight4"
copy "Source\Sparrow.Chart\Silverlight\Bin\Release\SL4\*.xml" "..\Build\Silverlight4"