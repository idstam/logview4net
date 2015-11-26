---
aliases:
 - /tag/viewer
 - /configure-viewer
author: "Johan Idstam"
date: 2010-02-28
title: "Configure Viewer"
weight: 25
wordpress_id: 24
categories:
- Documentation
tags:
- Configure
- Viewer
---

<img class="alignnone" alt="" src="/images/logview4net/TextViewerConfigurator.jpg" width="660" height="148" />


Here you can set how most of the events will look on screen.

**Remove surrounding whitespace** When checked whitespace around events will be removed befor showing them on screen.

**Cache events on pause** This will save events in memory and shown when you unpause the viewer.

**Show listener header **Some listeners add column headers to the events if you check this one.

**Save to file** If you check this **ALL** received events will be saved to a file before they are filtered or shown on screen. **Rolling** decides when the log file is closed and a new one created. This is to stop the file from becoming to big.

**Max number of characters in the viewer** When there are these many characters in the viewer 10% will be removed from the top. If you don't need a lot of history you can set this to a low number to save some memory.
