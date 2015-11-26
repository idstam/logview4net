call set_version.cmd

del "src\setup\logview4net.exe"

call build_release.cmd

"C:\Program Files\TrueCrypt\TrueCrypt.exe" /v "C:\Users\johan\Desktop\Dropbox\jsiSoft\secret_data.tc" /l x /q

ping 127.0.0.1 -n 5 > nul

cd  %logview4net_root%\src\App\bin\release

call x:\SignIt2.cmd logview4net.exe "logview4net <[version]>" sign_log_app.txt
type sign_log_app.txt


cd  %logview4net_root%\src\setup
call %nsis_path% logview4net.nsi

copy "logview4net_setup.exe" %logview4net_root% /y

cd  %logview4net_root%

call x:\SignIt2.cmd logview4net_setup.exe "logview4net <[version]> installer" sign_log_installer.txt
type sign_log_installer.txt

copy "logview4net_setup.exe" logview4net_%logview4net_file_version%_setup.exe
