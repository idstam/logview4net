/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using Microsoft.Win32;

namespace logview4net
{
    public class RegisterFiletype
    {

        public static bool Register(string appName, string fileExtension, string appDescription, string appPath)
        {
            try
            {
                if (!fileExtension.StartsWith("."))
                {
                    fileExtension = "." + fileExtension;
                }

                // Create a registry key object to represent the HKEY_CLASSES_ROOT registry section
                var rkRoot = Registry.ClassesRoot;

                // Attempt to retrieve the registry key for the XXX file type
                var rkFileType = rkRoot.OpenSubKey(fileExtension);

                // Was the file type found?
                if (rkFileType == null)
                {
                    // No, so register it
                    RegistryKey rkNew;

                    // Create the registry key
                    rkNew = rkRoot.CreateSubKey(fileExtension);

                    // Set the unique file type name
                    rkNew.SetValue("", appName);

                    // Create the file type information key
                    var rkInfo = rkRoot.CreateSubKey(appName);

                    // Set the default value to the file type description
                    rkInfo.SetValue("", appDescription);

                    // Create the shell key to contain all verbs
                    var rkShell = rkInfo.CreateSubKey("shell");

                    // Create a subkey for the "Open" verb
                    var rkOpen = rkShell.CreateSubKey("Open");

                    // Set the menu name against the key
                    rkOpen.SetValue("", "&Open Session");

                    // Create and set the command string
                    rkNew = rkOpen.CreateSubKey("command");
                    rkNew.SetValue("", appPath + " \"%1\"");

                    // Assign a default icon
                    rkNew = rkInfo.CreateSubKey("DefaultIcon");
                    rkNew.SetValue("", appPath + ",0");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
