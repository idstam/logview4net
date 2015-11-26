call set_version.cmd


del %logview4net_root%\logview4net_%logview4net_version%.online_help.zip


cd  %logview4net_root%\src\help\online
7za.exe a -r -tzip %logview4net_root%\logview4net_%logview4net_version%.online_help.zip *.*



cd  %logview4net_root%