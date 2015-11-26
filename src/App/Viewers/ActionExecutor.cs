/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;

namespace logview4net.Viewers
{
	/// <summary>
	/// Executes a non viewer action, preferably run in a thread of its own as a popup would block the viewer.
	/// </summary>
	public class ActionExecutor
	{
		private Action _action;
		private string _message;

		/// <summary>
		/// Creates a new <see cref="ActionExecutor"/> instance.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="message">Message.</param>
		public ActionExecutor(Action action, string message)
		{
			_action = action;
			_message = message;
		}

		/// <summary>
		/// Executes this instance.
		/// </summary>
		public void Execute()
		{
			switch(_action.ActionType)
			{
				case ActionTypes.PopUp:
					MessageBox.Show(_message,"logview4net message");
					break;
                case ActionTypes.PlaySound:
                    var sp = new SoundPlayer(_action.Data);
                    sp.Play();
                    break;
                case ActionTypes.Execute:
                    Process.Start(_action.Data);
                    break;

			}
		}
	}
}
