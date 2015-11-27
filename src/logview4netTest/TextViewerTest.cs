/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2008 Johan Idstam
 * 
 * 
 * This source code is released under the Artistic License 2.0.
 */
using System;
using logview4net.Viewers;
using NUnit.Framework;
using Action = logview4net.Viewers.Action;
using logview4net.Listeners;

namespace logview4net.test
{

    /// <summary>
    /// This is the TestFixture for TextViewerTest
	/// </summary>
	[TestFixture]
	public class TextViewerTest :TextViewer
	{
		/// <summary>
		/// Creates a new <see cref="TextViewerTest"/> instance.
		/// </summary>
		public TextViewerTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Check that the textbox can have the same amount of lines as the buffer size
		/// </summary>
		[Test]
		public void BufferEnforcementTestExactLineCount()
		{
			var tv = new TextViewerTest();
			Txt.Clear();
			var bs = 10;
			tv.BufferSize = bs;
            
			for(var i = 0; i < bs-1; i++)
			{
				tv.AddEvent(i.ToString(), new FileListener());
				//It seems that there is an empty line att the end if the 'last' line ends with a NewLine
				Assert.AreEqual(i +2, tv.Txt.Lines.Length, "Linecount differs from expected.");
			}
			
		}

		/// <summary>
		/// Check that the buffer size is enforced.
		/// </summary>
		[Test]
		public void BufferEnforcementTestOverrun()
		{
			var tv = new TextViewerTest();
			Txt.Clear();
			var bs = 11;
			tv.BufferSize = bs;

			for(var i = 0; i < bs; i++)
			{
				tv.AddEvent(i.ToString(), new FileListener());
			}
			//It seems that there is an empty line att the end if the 'last' line ends with a NewLine
			Assert.AreEqual(bs +1 , tv.Txt.Lines.Length, "Line count differs from expected.");
			
		}


        /// <summary>
        /// Tests the ignore action
        /// </summary>
        [Test]
        public void IgnoreActionTest()
        {
            Actions.Clear();
            Actions.Add(Action.CreateIgnoreAction("foo"));

            AddEvent("bar", new FileListener());
            Assert.AreEqual("bar\n", Txt.Text);
            AddEvent("foo", new FileListener());
            Assert.AreEqual("bar\n", Txt.Text);
            AddEvent("bar", new FileListener());
            Assert.AreEqual("bar\nbar\n", Txt.Text);
            
        }
    }
}