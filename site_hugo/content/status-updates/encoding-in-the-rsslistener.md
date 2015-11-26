---
aliases:
 - /encoding-in-the-rsslistener
author: "Johan Idstam"
date: 2013-06-12
title: Encoding in the RssListener
wordpress_id: 289
categories:
- Status
---

I just fixed a bug regarding character encoding in the RssListener.

The way I loaded the news feed into the XML parser broke the character encoding in the parser.

This also had me verify that right to left languages works as expected. 
Thanks to my coworker who happens to read the same language that prompted the bug report!

I don't have time to make a proper release for a couple of days due to my kids school ending for the summer.

Big thanks to the user who gave me the bug report.
