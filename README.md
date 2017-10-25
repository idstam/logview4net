# logview4net is a log viewer. 
It can monitor files, directories, incomming UDP/TCP traffic, the EventLog, SQL tables, 
Atom and RSS feeds.

**logview4net needs the .Net 4.5.2 Runtime to execute.**

## FEATURES:
  * Listens to a UDP port either in broadcast or single endpoint mode.
  * Monitors an eventlog, uses events on the local machine and polling on remote machines.
  * Monitors a log file or folder. Doing a tail using polling.
  * Monitors a table in a MS SQL-Server database. Doing a tail using polling.
  * Monitors RSS and Atom feeds. Strips most of HTML formatting from content.
  * Listens to a TCP port.
  * Monitors StdOut/StdErr of external application.
  * Can highlight or hide messages depending on content.
  * Supports multiple simultaneous listeners in each session.
  * Supports several simultaneous sessions (MDI).

## File hashes
  * Sha256 hash of installer: 89 b5 fb a6 90 41 f8 92 f5 87 03 77 48 51 8e 65 1a c9 1b 1e b4 5b 51 c6 2f 8a e5 32 66 48 92 ab
  * Sha256 hash of logview4net.exe: b7 e5 55 8b c5 bd 92 5b 85 6d 29 bd 31 15 67 5c 0d 70 20 ce b1 db 0a 45 05 05 a7 bc 77 43 72 5a

  To calculate the hash yourself in Windows execute the following: certutil -hashfile logview4net.exe sha256


## KNOWN BUGS:
  * The viewer configurator is not loaded with the right settings

## CHANGELOG:

### 17.08
  * Change .Net version to 4.5.2
  * Fix: An exception when loading configurations from file.

### 16.08
  * Fix Bug: #33 Log window is not displayed
  * Rewrote build pipeline in Python
  * Using Nuget to get the correct verson of MySql.Data
  * Fix: The button to restart a listener went out of sight

### 13.34
  * The main form now saves and restores its size and position.

### 13.25
  * Fix: Encoding problems in the RssListener
  * The installer is now signed with a verified certificate by Johan Idstam.

### 13.19
  * Fix: Multi line messages printed application class names instead of the actual content
  * Fix: Change of session title will now show in a running session

### 13.05
You can now drag/drop a file or folder into any textbox when configuring a listener to get the name of the dropped object.

### 13.01
  * Removed a binary component, a launcher, that wasn't doing anything useful.
  * Fixed bug in the rolling file storage that stopped polling listeners from running when there were more than one new line/message.

### 12.45
Small performance improvement in SqlListener and MySqlListener.

### 12.44
  * Added option to save all events to a rolling log file
  * Added an option to the viewer to skip the listener headers for messages. Only Tcp and Udp was affected.
  * Fixed bug: Not saving all config data for UserActions
  * Fixed request: An option to remove white space surrounding messages.

### 12.18
  * Added timestamp to MsSqlListener, MySqlListener and EventLogListener
  * Fixed some resizing when adding listeners to a session.

### 12.17
  * Added a user action that will execute any bat,cmd or exe when a pattern is found.
  * Added timestamp to UdpListener, FileListener, FolderListener, RssListener, 
		StdOutListener, TcpListner and UdpListener. 
		Using the date format documented here: 
		http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx

### 12.07
  * Added labels to the load/save button in the configure window
  * Added save to file context menu in the viewer window
  * Added match case and match word options to search dialog.


### 12.01
  * Fixed: [3467144] Filter displayed events (but do not ignore or remove them).
		Added an action that will filter out messages.
		Added a button in the toolbar to show all hidden messages.
		Added items in the pop up menu for a viewer to show hidden messages per action.
  * Fixed: Added MySql.data.dll to the install folder
  * Fixed: [3467137][2250823] The default font is "Courier new". But when one opens the font dialog then "Microsoft Sans Serif" is selected.
  * Fixed: [3467141] The Back color of the viewer is not take into account for user interaction/action configuration (text Example)

### 11.47
  * Fixed a bug where the text viewer ignored the 'Cache on pause' configuration option

### 11.14
  * Fixed a bug in MSSQL-Listener. The first row in a table was ignored if the user unchecked 'Start at end'
  * Added a MySQL listener kindly contributed by Balzás Botond

### 11.02
  * Fixed: [3048596] logview4net keeps redrawing view, even though it is Paused
  * Fixed: [2865511] F11 vill now hide/show the menu and the form border.
  * Fixed: An exception was sometimes thrown when closing a session window because the listener was not closed before the view.
  * Changed: The string pattern for an action is now a regular expression, not only an exact match.
  * Fixed: [3094649] tables list not retrieved when db name contains a period
  * Fixed: [2833781] HighlightMatch highlighting is offset by -1

### 9.38
  * Made the seach dialog to be not modal.
  * Added a 'Find Last' button to the search dialog.

### 9.31
  * Fixed: [2833781] HighLightMatch was of by one character

9.19
  * Fixed: [2793393] Added remote IP-address to the messages of the TCP-listener
  * Fixed: [2791880] Experimental TCP-listener. Please test it.
  * Fixed: [2777698] option to NOT display file data for FolderListener. The filename was not displayed when a file was changed.

### 9.17
  * Fixed: [2777698] option to NOT display file data for FolderListener

### 9.16
  * Fixed: [2770750] Text Foreground Color does not save correctly.
  * Fixed: [2770729] Settings Dont Stay

### 9.15
  * Fixed: [2250820] The part "User Interactions" in the settings dialog doesn't expand if one
adds more patterns.
  * Fixed: [2250843] Manage actions missing scrollbar
  * Fixed: [2250810] FolderListener: logview4net crashes if an additional file ...
  * Fixed: Http authentication in the RSSListener
  * Fixed: [2250834] Save dialog default folder
  * Fixed: [2250823] The default font is "Courier new". But when one opens the font dialog then"Microsoft Sans Serif" is selected.

### 8.46
  * Fixed: The config dialog is not shown when the user has started with a preselected config file and chooses 'New Session'
  * Fixed [2250817]: Minimize button on settings dialog removed.
  * Fixed [2250852]: Message when checking for updates
  * Fixed [2250830]: FileListener path name in config

### 8.38
  * Changed some code in the Highlighting code that fixed the 'of by some characters' coloring bug.
  * Fixed the vertical alignment of label texts in listener configurators.
  * Added basic authentication for the rss listener
  * Shows the initial items in reverse order for the rss listener.(Will probably add an option for this instead.)
  * Changed the caption on the rss poll interval label. The interval is minutes not milliseconds.
  * Forced poll interval on rss listener to be one or more.


### 8.24
  * The listeners prefixes are now part of the caption in the session config window. This is 
	so that the prefixes are visible even though the configurator is minimized.
  * Fixed [1876190]:Added some sleep to the file listener.

### 8.19
  * Added search dialog to text viewer. Reached by CTRL + F or right click.

### 8.03
  * The amount of memory used for the text buffer is now managed by the application.
	If there are any listeners with 'old' data. (Like a file listener where you 
	load all the data and not only tailing it.) It will truncate the data to ½GB
	when it reaches 1GB. If there are only tailing listeners then it will 
	truncate the data at 10MB to 5MB.
  * Changed license to Artistic License 2.0

### 8.02
  * Fixed: The progressbar was hidden after the first large file.
  * Request:Show show short filename on prefix-filename
  * Fixed: It is not possible to change the buffersize in the viewer config window.
  * Fixed: HighlightMatch only formatted the first occurance of the pattern in a message.
  * Fixed: The textbox reverts to default format when enforcing the buffer size
  * Added a Play Sound action that will play a PCM Wave File on pattern match.

### 8.01
  * Made it a lot faster to load existing data in files and SQL-tables.

### 7.49
  * The setting to add a filename to the prefix in the folder listener was lost in an earlier release.

### 7.43
  * Fixed: Actions where lost when fontsize had decimals.
  * Fixed: Create a new assembly for the listeners that are Microsoft Specific
	(EventLog and SQL-server for now)
  * Fixed for all but the MSListeners: Make all listener configurators use the new dynamic style and move them to non
	visual assembly.	

### 7.39
  * Added option to choose encoding in Udp-listener

### 7.28
  * Fixed: Add an IgnoreBlock action that should have an IgnoreStart + IgnoreEnd pattern
  * Fixed: Make it possible to ingore events on pause instead of caching them.
  * Fixed: Make the cmd-line parser use the StdOut-listener for .exe-files
  * Enabled the StdOut listener
  * Fixed a dispose bug on all listeners.

### 7.20
  * Added some command line configuration. Documented in the help file.
  * Made it possible to associate the .l4n extension with logview4net

7.19
  * Fixed: [1711956] When changing Poll timeout, settings not saved

### 7.18
  * RssListener moved to core.
  * StdOutListener moved to core (needs some more testing to go live)
  * FolderListener moved to core.
  * Fixed: The buttons in the config-form moves out of range when there are lots of listeners
  * Fixed & Handled: SecuritExceptions in EventLogListener on Vista
	There is now a manifetst embedded that elevates the application then
	it starts. I have also added some error handling to fail a bit nicer
	on most exceptions in the EventlogListener
  * Fixed: [1698479] When loading configuration, actions are lost
	Trying to parse font size as int
  * Added security elevation to the manifest file to make the app run with admin rights in Vista.
  * FileListener moved to core.
  * UdpListener moved to core.
  * Fixed: Make the installer know about .NET 2.0
  * Fixed bug when the program tried to look for updates and had no connection.

### 7.14
  * Fixed the bug when loading a configuration of a UdpListener
  * Done: Add check for updates, check only once a week (6 to 9 days)
  * UdpListener uses dynamic configurator.
  * Added a folder listener.
  * Moved tests to its own assembly
  * Moved Session to core assembly
  * Moved Logger to core assembly
  * Fixed: Configuration of session didn't work with the new look.
  * Fixed: Help - Documentation crashed the application if help.htm was deleted
  * Fixed: Add listener with no selected listener crashed the app

### 6.46.1
  * Fixed: Removing listeners from the session configuration window didn't remove them from the session.


### 6.46
  * New version numbering
	Since the version number don't really mean much I'll start using year.week instead. 
	If (however unlikely) I make two releases the same week I'll add a .number also.
  * Fixed: 'Add' button in ActionConfigurator miss aligned when there were no actions.
  * Fixed: Disable all context menu items in the viewer when nothing is selected.
  * Done: Give Load/Save buttons on configure form icons instead of text
  * Done: Create a new ConfigureSession form.
  * Done: Load logger settings from app.config
  * Fixed: Add an icon or text to the font button on action configurators	
  * Fixed: The 'only tail' option doesn't work for feeds
  * Done: Add a contect-menu to the text viewer that allows for fast creation of actions.
  * Fixed: Make the RssListener publish only 'new' feed items.
  * Done: Accept filenames to monitor as command line arguments. (.exe-files will use the new stdOut-listener all other the old file-listener)
  * Fixed: Make the RssListener handle Atom
  * Fixed: Action configuration wasn't properly updated when changing format on existing action.
  * Fixed: Now the font information from the actions is applied in the viewer.
  * Done: Changed the ActionConfigurator
		Removed colorpicker
		Added button to configure font  (including color)
  * Fixed: Sort actions on Pattern + Action priority (Ignore, HighlightMatch, HighLight, PopUp)
  * Done: New Action - HighlightMatch - Changes color  (and font) on a matchin string inside an event.
  * Done: Execute multiple actions.
  * Done: New listener - RssListener, will monitor an rss feed (2.0, 0.92, Atom tested).

### 1.2
  * I messed up the version number a bit. The released 1.2 has all from the 1.1 Changelog.

### 1.1
  * Festure request 1489611: Scroll all windows
  * Moved project from CVS to Subversion on SourceForge (060317)
  * Changed default file extension for loading/saving configurations to *.xml
  * Added a toolbar button to toggle word wrap in the current viewer.
  * Changed the toolbar icons to use Silk Icons from: http://www.famfamfam.com/lab/icons/silk/
  * Actions can now be moved up & down in the action manager.

### 1.0
  * Fixed: Actions doesn't appear when configuring a running session.
  * Fixed:Headers missing in ActionManager when there are no actions.
  * Fixed:Values are not updated in listener configurators when loading settings from file.
  * Fixed: The ForeColor was not saved correctly when saving session configuration to file.


### 1.0RC1
  * Allows some changes to listeners in a running session.
  * Allows changes to all parameters of a viewer in a running session.
  * Now is can Save/Load the configuration file
  * Fixed bug: Listeners to a runnig session wasn't started.
  * Fixed (sort of): Changes to the viewer in the Session Configurator isn't reverted when 
pressing 'Cancel' (Removed the Cancel button)

### 0.9.1
  * Fixed bug that prevented the use of EventLogListener.

### 0.9
  * Uses .NET 2.0
  * Uses MDI Windows instead of tabs.
  * Added a MS SQL-Server listener
  * Lots of internal changes.

### 0.6
  * Fixed bug when removing a listener in the config form.

### 0.5
  * Removed some Acions and implemented the PopUp Action.
  * Fixed the internals of configuring a session
  * Fixed a threading issue with multiple listeners
  * Fixed saving the configuration
  * Added functionality for all the buttons in the main toolbar

### 0.4
  * Fixed the session configuration dialog.
