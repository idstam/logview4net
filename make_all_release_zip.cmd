cd %logview4net_root%
call make_bin_release_zip.cmd

cd %logview4net_root%
call make_source_release_zip.cmd

cd %logview4net_root%
call make_source_no_lib_release_zip.cmd


cd %logview4net_root%
call make_setup_release_zip.cmd

cd  %logview4net_root%