@echo off
cd ..
cd Build
mkdir 4.0
mkdir 3.5
mkdir Silverlight4
mkdir Silverlight5
cd ..
cd Work
copy  Document\Help\*.chm ..\Build\4.0
copy  Source\Sparrow.Chart\WPF\bin\Release\*.xml ..\Build\4.0
copy  Source\Sparrow.Chart\WPF\bin\Release\*.dll ..\Build\4.0
copy  Source\Sparrow.Chart\WPF\bin\Release\3.5\*.xml ..\Build\3.5
copy  Source\Sparrow.Chart\WPF\bin\Release\3.5\*.dll ..\Build\3.5
copy Source\Sparrow.Chart\Silverlight\Bin\Release\*.dll ..\Build\Silverlight5
copy Source\Sparrow.Chart\Silverlight\Bin\Release\*.xml ..\Build\Silverlight5
copy Source\Sparrow.Chart\Silverlight\Bin\Release\4.0\*.dll ..\Build\Silverlight4
copy Source\Sparrow.Chart\Silverlight\Bin\Release\4.0\*.xml ..\Build\Silverlight4