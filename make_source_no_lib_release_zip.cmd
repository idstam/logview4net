call set_version.cmd

xcopy src dep_src /s


rmdir dep_src\app\bin /s /q
rmdir dep_src\app\obj /s /q


rmdir dep_src\setup\debug /s /q
rmdir dep_src\setup\release /s /q

rmdir dep_src\logview4net.mslisteners\bin /s /q
rmdir dep_src\logview4net.mslisteners\obj /s /q

rmdir dep_src\logview4net.core\bin /s /q
rmdir dep_src\logview4net.core\obj /s /q

rmdir dep_src\logview4netTest\bin /s /q
rmdir dep_src\logview4netTest\obj /s /q

rmdir dep_src\Test\bin /s /q
rmdir dep_src\Test\obj /s /q

del dep_src\Deployment\*.exe
del dep_src\Setup\*.exe
del dep_src\BuildRelease.log



del logview4net.src.zip

7za.exe a -r -tzip %logview4net_root%\logview4net_%logview4net.src.zip dep_src\*.*


rmdir dep_src /s /q

cd  %logview4net_root%