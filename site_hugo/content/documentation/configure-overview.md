---
aliases:
 - /tag/overview
 - /configure-overview
author: "Johan Idstam"
date: 2010-02-28
title: "Configure Overview"
weight: 1
wordpress_id: 12
categories:
- Documentation
tags:
- Configure
- Overview
---

<img class="alignnone" src="/images/logview4net/ConfigureLogSession.jpg" alt="" width="486" height="241" />



This is where you configure a log session.



	
  * Session title: This is the window title of the session.

	
  * [Listeners:](/documentation/listeners-overview) Choose what kind of log listener you want to use in the drop down list  and click 'Add'. You will be presented with the relevant options for the  selected type of listener. You can add as many listeners to one session as you want as long as they  are not in conflict with each other. This will for example happen if  you try to add two UDP Listeners that listen on the same port.

	
  * [Viewer:](/documentation/configure-viewer) This is where you configure the viewer.

	
  * Load: Cick here to load a saved log session configuration.

	
  * Save: Click here to save the current log session configuration. If you save it as DefaultSession.xml in the application folder it will  be loaded when starting the application.

	
  * OK: Click here to start the session.


