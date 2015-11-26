---
aliases:
 - /tag/mysql
 - /mysql-listener
author: "Johan Idstam"
date: 2011-04-03
title: "MySql Listener"
weight: 130
wordpress_id: 108
categories:
- Documentation
tags:
- MySql
---

	<img class="alignnone" src="/images/logview4net/SqlListenerConfigurator.jpg" alt="" width="464" height="122" />



MySql Listener does a tail on a table in a MySql Database. The documentation is a copy of the MSSQL-Listener. Please comment if you have questions or want some changes.



	
  * Prefix: A small text that is shown before the event in the viewer to indicate  where the event came from.

	
  * Server: Hostname or IP of the SQL-Server

	
  * User: Username to use for login.

	
  * Password: Password to use for login.

	
  * Database: The databes to use.

	
  * Table: The table to watch.

	
  * Max Column: The column to SELECT MAX from to find new rows.

	
  * Poll intervall: How often, in milliseconds, to check the table for new rows.

	
  * Start at end: When this is checked only new items are showed. When it is not checked  the listener starts with getting all rows in the table.
