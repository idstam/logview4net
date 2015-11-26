---
aliases:
 - /tag/eventlog
 - /eventlog-listener
author: "Johan Idstam"
date: 2010-02-28
title: "Eventlog Listener"
weight: 100
wordpress_id: 46
categories:
- Documentation
tags:
- Eventlog
- Listener
---

<img class="alignnone" src="/images/logview4net/EventLogListenerConfigurator.jpg" alt="" width="464" height="65" />


EventLog Listener monitors an event log.



	
  * Host IP: IP address of the machine hosting the event log.

	
  * Log name: The eventlog to monitor.

	
  * Poll intervall: How often, in milliseconds, the eventlog should be checked. This is only  used when not monitoring the local machine. The listener uses events  for local event logs.

	
  * Append field names: Whether or not to append the eventlog field names to the log data.

	
  * Prefix: A small text that is shown before the event in the viewer to indicate  where the event came from.


