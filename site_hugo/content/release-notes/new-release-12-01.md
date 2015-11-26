---
aliases:
 - /new-release-12-01
author: "Johan Idstam"
date: 2012-01-01
title: New Release 12.01
wordpress_id: 130
categories:
- Release
---

Starting the new year with a release.

All thanks to [Olivier](https://sourceforge.net/users/olivierwsgd/) who made some nice bug reports and sugested a new Action.

The new Action is called Hide and lets you hid messages until you want to see them. It works a litle bit like the Payse button, but uses a regulas expresson to trigger just like all other actions.

To show all hidden messages; press the tool bar button names Hidden.

To show only messages hidden for a particular regular expression; right klick in the text area and select the action you want messages shownd for in the pop up menu.

**Here are the release notes for logview4net 12.01:**


* Fixed: [3467144] Filter displayed events (but do not ignore or remove them).
* Added an action that will filter out messages.
* Added a button in the toolbar to show all hidden messages.
* Added items in the pop up menu for a viewer to show hidden messages per action.
* Fixed: Added MySql.data.dll to the install folder
* Fixed: [3467137][2250823] The default font is "Courier new". But when one opens the font dialog then "Microsoft Sans Serif" is selected.
* Fixed: [3467141] The Back color of the viewer is not take into account for user interaction/action configuration (text Example)
