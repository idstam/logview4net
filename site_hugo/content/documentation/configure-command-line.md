---
aliases:
 - /tag/command-line
 - /configure-command-line
author: "Johan Idstam"
date: 2010-02-28
title: "Configure Command-line"
weight: 55
wordpress_id: 30
categories:
- Documentation
tags:
- Command line
- Configure
---

You can put any amount (within reason)  of filenames on the command-line when starting the application. If there  are filenames on the commandline all listeners in the default session  will be removed and new ones will be created as described below.



	
  * A file name with the .l4n extension  will remove the [default  session](/documentation/default-configuration) and start a new according to the settings in the file. The  configuration window will not be shown on startup. Make sure this is the  first argument on the command line if you intend to add more arguments.

	
  * A folder name will create a [folder listener](/documentation/folder-listener) for  that folder.

	
  * A file name with the .exe extension will start the application with a predefined [StdOut listener](/documentation/stdout-stderr-listener).

	
  * Any other file name will create a [file listener](/documentation/file-listener) for  that file.


