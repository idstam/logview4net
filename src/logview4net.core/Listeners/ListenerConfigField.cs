/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System.Collections.Generic;

namespace logview4net.Listeners
{
    public enum MultiValueTypes
    {
        None,
        Combo,
        Options,
        Check,
        Linebreak,
        FileOpenButton,
        FolderBrowserButton,
        Password
    }

    public class ListenerConfigField
    {
        public string Name;
        public List<string> MultiOptions = null;
        public MultiValueTypes MultiValueType = MultiValueTypes.None;
        public string Value;
        public int Width = -1;
        public string AlignTo = "";
        public string ToolTip = "";

    }
}
