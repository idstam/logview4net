/*
 * Created by SharpDevelop.
 * User: johan
 * Date: 2012-10-30
 * Time: 21:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;

namespace logview4net
{
	/// <summary>
	/// Description of RollingFileAppender.
	/// </summary>
	public class RollingFileAppender
	{
		private string _baseFileName = null;
		private string _rollingType;
		public string _lastDatePart; //Public to enable testing
		private bool _enabled = false;
		
		public RollingFileAppender(string logFile, string logRolling, bool enabled)
		{
			_enabled = enabled;
			_baseFileName = logFile;
			_rollingType = logRolling.Trim();
			
			switch(_rollingType)
			{
				case "Daily":
					_lastDatePart = DateTime.Now.ToString("yyyyMMdd");
					break;
				case "Hourly":
					_lastDatePart = DateTime.Now.ToString("yyyyMMddhh");
					break;
			}
		}
		
		
		public void Write(string msg)
		{
			if(!_enabled) return;
			
			try
			{
				rollFile();
				File.AppendAllText(_baseFileName, msg + Environment.NewLine);
			}
			catch(Exception ex)
			{
				Logger.GetLogger("RollingFileAppender").Error(GetHashCode(), "Error when Rolling log file", ex);
			}
			
		}

		void rollFile()
		{
			//If there is no file there yet there is ni need to roll it.
			if(! File.Exists(_baseFileName)) return;
			
			try
			{
				var curDatePart = "";
				switch (_rollingType)
				{
					case "Daily":
						curDatePart = DateTime.Now.ToString("yyyyMMdd");
						if (_lastDatePart != curDatePart) {
							rollFileOnDate(curDatePart);
						}
						break;
					case "Hourly":
						curDatePart = DateTime.Now.ToString("yyyyMMddhh");
						if (_lastDatePart != curDatePart) {
							rollFileOnDate(curDatePart);
						}
						break;
					case "100 KB":
						if (new FileInfo(_baseFileName).Length >= (100 * 1024)) {
							rollFileOnSize();
						}
						break;
					case "1 MB":
						if (new FileInfo(_baseFileName).Length >= (1024 * 1024)) {
							rollFileOnSize();
						}
						break;
					case "10 MB":
						if (new FileInfo(_baseFileName).Length >= (10 * 1024 * 1024)) {
							rollFileOnSize();
						}
						break;
				}
			}
			catch(Exception ex)
			{
				Logger.GetLogger("RollingFileAppender").Error(GetHashCode(), "Error when Rolling log file", ex);
			}
		}
		
		void rollFileOnSize()
		{
			var fileName = getUnusedFileName(_baseFileName);
			File.Move(_baseFileName, fileName);
		}
		
		void rollFileOnDate(string datePart)
		{
			_lastDatePart = datePart;
			var fileName = _baseFileName + "." + datePart;
			
			fileName = getUnusedFileName(fileName);
			
			File.Move(_baseFileName, fileName);
		}
		
		private string getUnusedFileName(string fileName)
		{
			if(! File.Exists(fileName))
			{
				return fileName;
			}
			string ret;
			var i = 0;
			do
			{
				i++;
				ret = fileName + "." + i.ToString();
				
			} while(File.Exists(ret));
			
			return ret;
		}
	}
}
