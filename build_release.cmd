call set_version.cmd

rmdir src\app\bin\release /s /q
rmdir src\app\obj /s /q


rmdir src\setup\release /s /q

cd src

C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:Clean,Build /property:Configuration=Release /fileLogger logview4net.sln
REM %devenv_path% logview4net.sln /rebuild RELEASE /out BuildRelease.log

cd  %logview4net_root%
