call set_version.cmd


del logview4net_%logview4net_version%.bin.zip

call build_release.cmd

cd %logview4net_root%\src\app\bin\release\
7za.exe a -r -tzip %logview4net_root%\logview4net_%logview4net_version%.bin.zip *.*

cd %logview4net_root%\src\app\
7za.exe a -r -tzip %logview4net_root%\logview4net_%logview4net_version%.bin.zip readme.txt

cd  %logview4net_root%\src\app\
7za.exe a -r -tzip %logview4net_root%\logview4net_%logview4net_version%.bin.zip license.txt

cd  %logview4net_root%