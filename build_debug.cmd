call set_version.cmd

rmdir src\app\bin\debug /s /q
rmdir src\app\obj /s /q


rmdir src\setup\debug /s /q

cd src

%devenv_path% logview4net.sln /rebuild DEBUG /out BuildRelease.log

cd  %logview4net_root%
