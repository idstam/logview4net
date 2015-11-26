---
author: johanidstam
comments: true
date: 2011-09-16
title: Help me with requirements for column headers
wordpress_id: 116
categories:
- Inspiration
---

I got a request a while back to add column headers for the database listeners.

At the first glance it might seem like a simple thing to implement, so I did like I usually do and opened my code editor, but reality came around fast as lightning.

So, now dear users I need some help figuring out how column headers should work.

**PLEASE write a comment to this post if you want to affect the outcome of column headers are implemented in logview4net.**

I'll list the gotchas that I've found this far, but I'll keep my current idea for a solution out of here for a while 'cause it's not very good.



	
  * There can be more than one active listener in a viewer, writing messages in unpredictable order.

	
  * Any column headers will scroll of the screen eventually.

	
  * Writing column headers before each new message makes the ouput unreadable.



Here's your chance to prove open source is superior. Bring it on.
