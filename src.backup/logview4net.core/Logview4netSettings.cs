/*
 * Created by SharpDevelop.
 * User: johan
 * Date: 2012-11-08
 * Time: 09:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Configuration;

namespace logview4net.core
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public sealed class Logview4netSettings
	{
		public static ApplicationSettingsBase Instance
		{
			get;set;
		}
		
	}
}
