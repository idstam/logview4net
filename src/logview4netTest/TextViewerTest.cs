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
	public class TextViewerTest
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
            var tv = new TextViewer();

            tv.Txt.Clear();
            var bs = 10;
            tv.BufferSize = bs;

            for (var i = 0; i < bs - 1; i++)
            {
                tv.AddEvent(i.ToString(), new FileListener());
                //It seems that there is an empty line att the end if the 'last' line ends with a NewLine
                Assert.Less(tv.Txt.Lines.Length, 7,
                    "There should be less than seven lines since the buffer is 10 characters");
            }
        }

        /// <summary>
		/// Check that the buffer size is enforced.
		/// </summary>
		[Test]
		public void BufferEnforcementTestOverrun()
		{
			var tv = new TextViewer();
            tv.Txt.Clear();
			var bs = 11;
			tv.BufferSize = bs;

			for(var i = 0; i < bs; i++)
			{
				tv.AddEvent(i.ToString(), new FileListener());
			}
			//It seems that there is an empty line att the end if the 'last' line ends with a NewLine
			Assert.LessOrEqual(tv.Txt.TextLength, bs , "There should be less or equal amount characters ass the buffer size. With an occational newline to much.");
			
		}


        /// <summary>
        /// Tests the ignore action
        /// </summary>
        [Test]
        public void IgnoreActionTest()
        {
            var tv = new TextViewer();
            tv.Actions.Clear();
            tv.Actions.Add(Action.CreateIgnoreAction("foo"));

            tv.AddEvent("bar", new FileListener());
            Assert.AreEqual("bar\n", tv.Txt.Text);
            tv.AddEvent("foo", new FileListener());
            Assert.AreEqual("bar\n", tv.Txt.Text);
            tv.AddEvent("bar", new FileListener());
            Assert.AreEqual("bar\nbar\n", tv.Txt.Text);
            
        }
    }
}