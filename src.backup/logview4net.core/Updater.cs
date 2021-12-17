/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using logview4net.core;

namespace logview4net
{
    public class Updater
    {
        private int _currentVersion;
        private int _availableVersion;
        private string _fileUrl;
        private string _manifestUrl;
        private ILog _log = Logger.GetLogger(typeof(Updater));
        private bool _forced = false;

        public Updater()
        {
        	_manifestUrl = (string)Logview4netSettings.Instance["ManifestUrl"];
            GetCurrentVersion();

        }

        public Updater(bool forced):this()
        {
            _forced = forced;
            if(_forced)
            {
                Logview4netSettings.Instance["NextManifestCheck"] = DateTime.Now.AddDays(-1);
                Logview4netSettings.Instance.Save();
                
            }
        }

        public void CheckUpdates()
        {
            var ts = new ThreadStart(DoUpdate);
            var t = new Thread(ts);
            t.Start();
        }
        public void CheckUpdatesFromGui()
        {
            if (HasUpdate())
            {
                DoUpdate();
            }
            else
            {
                MessageBox.Show("There is no update available.");

            }
        }

        private void DoUpdate()
        {
            _log.Debug(GetHashCode(), "DoUpdate");
            
#if FALSE//DEBUG
            _log.Debug(GetHashCode(), "Skipping update due to debug mode.");
            return;
#else
            if (HasUpdate())
            {
                var msg = "There is a newer version of " + Application.ProductName + " available." + Environment.NewLine + Environment.NewLine + "Do you want to download it now?";

                if (MessageBox.Show(msg, "Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FetchUpdate();
                    var r = new Random();
                    Logview4netSettings.Instance["NextManifestCheck"] = DateTime.Now.AddDays(r.Next(6, 10));
                }
                else
                {
                	Logview4netSettings.Instance["NextManifestCheck"] = DateTime.Now.AddDays(3);
                }
                Logview4netSettings.Instance.Save();
            }
            
#endif

        }

        private bool HasUpdate()
        {
            _log.Debug(GetHashCode(), "HasUpdate");

            if (_manifestUrl == null || _manifestUrl == "")
            {
                //If there is no url fake the result.
                return false;
            }

            if( (DateTime)Logview4netSettings.Instance["NextManifestCheck"] > DateTime.Now)
            {
                return false;
            }

            //Get the manifest;
            var wc = new WebClient();

            try
            {
                var manifestData = wc.DownloadString(_manifestUrl);
                var sr = new StringReader(manifestData);
                sr.ReadLine(); //Header
                sr.ReadLine(); //Header
                _manifestUrl = sr.ReadLine(); //Next manifest url
                _log.Debug(GetHashCode(), "Next manifest url=" + _manifestUrl);
                sr.ReadLine(); //Header
                var foo = sr.ReadLine(); //Version
                _availableVersion = int.Parse(foo);
                _log.Debug(GetHashCode(), "Available version==" + _availableVersion.ToString());
                sr.ReadLine(); //Header
                _fileUrl = sr.ReadLine(); //File url
                _log.Debug(GetHashCode(), "File url==" + _fileUrl);
                Logview4netSettings.Instance["ManifestUrl"] = _manifestUrl;
                Logview4netSettings.Instance.Save();

                return _currentVersion < _availableVersion;

            }
            catch(Exception x)
            {
                _log.Warn(GetHashCode(), "Trying to fetch version manifest.", x);
                return false;
            }
            finally
            {
                wc.Dispose();
            }
        }

        private void GetCurrentVersion()
        {
            _log.Debug(GetHashCode(), "GetCurrentVersion");

            _currentVersion = Assembly.GetCallingAssembly().GetName().Version.Major *1000;
            _currentVersion += Assembly.GetCallingAssembly().GetName().Version.Minor *10;
            _currentVersion += Assembly.GetCallingAssembly().GetName().Version.Revision;

            _log.Debug(GetHashCode(), "GetCurrentVersion=" + _currentVersion.ToString());
        }

        private void FetchUpdate()
        {
            _log.Debug(GetHashCode(), "FetchUpdate from "+ _fileUrl);

            Process.Start(_fileUrl);
        }

        //Manifest file:
        //ProdName auto update manifest
        //Next manifest url=......
        //Available version=..... (7050)
        //File url=........
        //
    }
}
