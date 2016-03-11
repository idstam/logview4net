using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace logview4net.test
{
    [TestFixture]
    public class HexEncoderTest
    {
        public HexEncoderTest()
        {

        }
        [Test]
        public void HexTest_OneLineAtZero()
        {
            var he = new  logview4net.Listeners.HexEncoder();
            var b = new byte[]{65, 65, 65, 65, 65, 65, 65, 65, 
                                66, 66, 66, 66, 66, 66, 66, 66};

            var ret = he.GetHex(0, b);

            Assert.AreEqual("00000000h: 41 41 41 41 41 41 41 41 42 42 42 42 42 42 42 42 ; AAAAAAAABBBBBBBB", ret);
        }
        [Test]
        public void HexTest_TwoLines()
        {
            var he = new logview4net.Listeners.HexEncoder();
            var b = new byte[]{65, 65, 65, 65, 65, 65, 65, 65, 66, 66, 66, 66, 66, 66, 66, 66, 
                               65, 65, 65, 65, 65, 65, 65, 65, 66, 66, 66, 66, 66, 66, 66, 66};

            var hex = he.GetHex(0, b);
            var ret = getLines(hex);
            //Assert.AreEqual("00000000h: 41 41 41 41 41 41 41 41 42 42 42 42 42 42 42 42 ; AAAAAAAABBBBBBBB", ret[0]);
            Assert.AreEqual("00000010h: 41 41 41 41 41 41 41 41 42 42 42 42 42 42 42 42 ; AAAAAAAABBBBBBBB", ret[1]);
        }

        [Test]
        public void HexTest_OneAndHalf()
        {
            var he = new logview4net.Listeners.HexEncoder();
            var b = new byte[]{65, 65, 65, 65, 65, 65, 65, 65, 66, 66, 66, 66, 66, 66, 66, 66, 
                               65, 65, 65, 65, 65, 65, 65, 65};

            var hex = he.GetHex(0, b);
            var ret = getLines(hex);
            //Assert.AreEqual("00000000h: 41 41 41 41 41 41 41 41 42 42 42 42 42 42 42 42 ; AAAAAAAABBBBBBBB", ret[0]);
            Assert.AreEqual("00000010h: 41 41 41 41 41 41 41 41                         ; AAAAAAAA", ret[1]);
        }

        private List<string> getLines(string s)
        {
            var ret = new List<string>();
            using(var sr = new StringReader(s))
            {
                string l = "";
                while(l != null)
                {
                    l = sr.ReadLine();
                    ret.Add(l);
                }
            }
            return ret;
        }
        [Test]
        public void HexTest_LowChars()
        {
            var he = new  logview4net.Listeners.HexEncoder();
            var b = new byte[]{15, 25, 32, 33, 5, 5, 5, 5, 
                                6, 6, 6, 6, 6, 6, 6, 6};

            var ret = he.GetHex(10, b);
            string notPrintable12 = new string((char)176, 12);
            string notPrintable2 = new string((char)176, 2);
            string expeted = "0000000Ah: 0F 19 20 21 05 05 05 05 06 06 06 06 06 06 06 06 ; " + notPrintable2 + " !" +
                             notPrintable12;
            Assert.AreEqual(expeted, ret);
        }
    }
}
