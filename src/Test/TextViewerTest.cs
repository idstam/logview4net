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

namespace logview4net.test
{

    /// <summary>
    /// This is the TestFixture for TextViewerTest
	/// </summary>
	[TestFixture]
	public class TextViewerTest :textViewer
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
			TextViewerTest tv = new TextViewerTest();
			txt.Clear();
			int bs = 10;
			tv.BufferSize = bs;

			for(int i = 0; i < bs-1; i++)
			{
				tv.AddEvent(i.ToString());
				//It seems that there is an empty line att the end if the 'last' line ends with a NewLine
				Assert.AreEqual(i +2, tv.txt.Lines.Length, "Linecount differs from expected.");
			}
			
		}

		/// <summary>
		/// Check that the buffer size is enforced.
		/// </summary>
		[Test]
		public void BufferEnforcementTestOverrun()
		{
			TextViewerTest tv = new TextViewerTest();
			txt.Clear();
			int bs = 11;
			tv.BufferSize = bs;

			for(int i = 0; i < bs; i++)
			{
				tv.AddEvent(i.ToString());
			}
			//It seems that there is an empty line att the end if the 'last' line ends with a NewLine
			Assert.AreEqual(bs +1 , tv.txt.Lines.Length, "Line count differs from expected.");
			
		}


        /// <summary>
        /// Tests the ignore action
        /// </summary>
        [Test]
        public void IgnoreActionTest()
        {
            Actions.Clear();
            Actions.Add(Action.CreateIgnoreAction("foo"));

            AddEvent("bar");
            Assert.AreEqual("bar\n", txt.Text);
            AddEvent("foo");
            Assert.AreEqual("bar\n", txt.Text);
            AddEvent("bar");
            Assert.AreEqual("bar\nbar\n", txt.Text);
            
        }
    }
}