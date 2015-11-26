---
aliases:
 - /tag/action
 - /tag/configure
 - /tag/ignore
 - /tag/popup
 - /configure-actions
author: "Johan Idstam"
comments: true
date: 2010-02-28
title: "Configure Actions"
weight: 30
wordpress_id: 26
categories:
- Documentation
tags:
- Action
- Configure
- Highlight
- Ignore
- Popup
---

	<img class="alignnone" src="/images/logview4net/Actions.jpg" alt="" width="424" height="189" />



Actions are instructions to the viewer how it should react to  different events. When an event is received by the viewer it looks through the associated  actions from the top and applies the first matching action. An action is concidered a match when its pattern matches one or more  words in the log event.



	
  * Pattern: A string to match against the text of an event

	
  * Action type: What to do when an event matches the pattern.

	
  * Color: The fore color to use for the hightlight action.

	
  * Delete: Press the Delete button corresponding to an actions you want to remove  from the current session.

	
  * Add: Adds an action to the current session.




#### Highlight


Enables coloring of an event. 


#### Ignore


Makes the event not to appear in the  viewer.


#### Popup


Pops up a messagebox with the event  data.


#### Highlight match


Enables coloring of the matched  content inside the event.


#### Start ignore


Tells the viewer to start ignoring  all events until it receives an End Ignore action.


#### End ignore


Tells the viewer to start showing  events again after ignoring them. The events are not counted so one End  will terminate the ignoring even if there has been five Start actions.
