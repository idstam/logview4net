/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * This source code is released under the Artistic License 2.0.
 */

using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace logview4net.Viewers
{
			
	/// <summary>
	/// Some helper functions to be used by implementors of <see cref="IViewer"/>.
	/// </summary>
	public class ViewerUtils
	{
		/// <summary>
		/// These are reasons for ignoring a message
		/// </summary>
		public enum IgnoreReasons
		{
			/// <summary>
			/// Beginning af an ignore block
			/// </summary>
			StartedBlock,
			/// <summary>
			/// End of an ignore block
			/// </summary>
			EndedBlock,
			/// <summary>
			/// Inside an ignore block
			/// </summary>
			InBlock,
			/// <summary>
			/// An explicitly ignored message.
			/// </summary>
			Ignore,
			/// <summary>
			/// The message should not be ignored.
			/// </summary>
			DoNotIgnore,
			/// <summary>
			/// The message should hidden. To be shown later.
			/// </summary>
			Hide
			
			    
		}
		private static volatile bool _inIgnoreBlock = false;
		
		/// <summary>
		/// Creates a new <see cref="ViewerUtils"/> instance.
		/// </summary>
		public ViewerUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}



		/// <summary>
		/// Gets a List of <see cref="Action"/> that is applicable for the current message
		/// </summary>
		/// <param name="actions">An <see cref="ArrayList"/> with <see cref="Action"/> </param>
		/// <param name="data">The pattern to match.</param>
		/// <returns></returns>
		public static List<Action> GetActions(List<Action> actions, string data)
		{

            try
            {
                var ret = new List<Action>();

                foreach (var a in actions)
                {
                    if (a.IsMatch(data))
                    {
                        ret.Add(a);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw ExceptionManager.HandleException(0, ex);
            }

		}

		/// <summary>
		/// Executes the non viewer action.
		/// </summary>
		public static void ExecuteNonViewerAction(Action action, string data)
		{
			var ae = new ActionExecutor(action, data);
			
			var ts = new ThreadStart(ae.Execute);
			var t = new Thread(ts);
			t.Start();

		}
		/// <summary>
		/// Decides if the event should be ignored
		/// </summary>
		/// <returns>True if the event should be ignored.</returns>
		public static bool IgnoreEvent(LogEvent le, out IgnoreReasons reason)
		{
			var ret = false;
			reason = IgnoreReasons.DoNotIgnore;
			
			foreach(var a in le.Actions)
			{
			    switch(a.ActionType)
			    {
			        case ActionTypes.StartIgnoreBlock:
    					_inIgnoreBlock = true;
    					reason = IgnoreReasons.StartedBlock;
			            break;
			        case ActionTypes.EndIgnoreBlock:
    					_inIgnoreBlock = false;
    					ret = true; //This is to supress the end message
    					reason = IgnoreReasons.EndedBlock;
			            break;
			        case ActionTypes.Hide:
			            a.HideCache.Add(le.Message);
                        reason = IgnoreReasons.Hide;
			            ret = true;
			            break;
			                
			    }
				
				//If it's an ignore event return true
				if(! ret)
				{
					ret = (a.ActionType == ActionTypes.Ignore);
					if(reason == IgnoreReasons.DoNotIgnore)
					{
						reason = IgnoreReasons.Ignore;
					}
				}
			}

				//If ignore counter > 0 then return true but is ret already is true don't change it.				
				if(! ret)
				{
					ret = _inIgnoreBlock;
					if(reason == IgnoreReasons.DoNotIgnore )
					{
						reason = IgnoreReasons.InBlock;
					}
				}

			return ret;			
				
			
		}
	}
}
