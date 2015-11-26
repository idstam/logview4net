---
aliases:
 - /tag/ms-sql
 - /ms-sql-listener
author: "Johan Idstam"
date: 2010-02-28
title: "MS-SQL Listener"
weight: 120
wordpress_id: 44
categories:
- Documentation
tags:
- Listener
- MS-SQL
---

<img class="alignnone" src="/images/logview4net/SqlListenerConfigurator.jpg" alt="" width="464" height="122" />


MS-SQL Listener does a tail on a table  in a Microsoft SQL Server Database



	
  * Prefix: A small text that is shown before the event in the viewer to indicate  where the event came from.

	
  * Server: Hostname or IP of the SQL-Server

	
  * User: Username to use for login.

	
  * Password: Password to use for login.

	
  * Windows Authentication: Check this if you want to use windows integrated security to login.

	
  * Database: The databes to use.

	
  * Table: The table to watch.

	
  * Max Column: The column to SELECT MAX from to find new rows.

	
  * Poll intervall: How often, in milliseconds, to check the table for new rows.

	
  * Start at end: When this is checked only new items are showed. When it is not checked  the listener starts with getting all rows in the table.


