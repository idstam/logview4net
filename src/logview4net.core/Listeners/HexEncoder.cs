using System;
using System.Collections.Generic;
using System.Text;

namespace logview4net.Listeners
{
    public class HexEncoder
    {
        public static string EncName = "-- HEX/ASCII --";
        //How mqany bytes should there be on one line
        public int BlockLength = 16;
        private string _hexChars = "0123456789ABCDEF";
        public HexEncoder()
        {

        }

        public string GetHex(ulong startAddress, byte[] buffer)
        {
            int start = 0;
            var ret = new StringBuilder();

            for (int l = 0; l <= buffer.Length / BlockLength; l++)  // '<=' instead of '<' to get the last bytes when they don't align to the block size
            {
            	start = l *BlockLength;
                
                if (start >= buffer.Length) break;

                if (l != 0) ret.AppendLine();

            	int end = start + BlockLength;
                ret.Append(getHexAddress(startAddress));
                string ascii = "";
                for(int i = start; i < end; i++)
                {
                	if(i >= buffer.Length) break;
                	
                	ret.Append(byteToHex(buffer[i]));
                	ret.Append(" ");
                	if(buffer[i] > 31)
                	{
                		ascii += Encoding.GetEncoding("ISO-8859-1").GetString(buffer, i, 1);
                	}
                	else
                	{
                		ascii += (char)176;
                	}
                }
                if(ascii.Length < BlockLength)
                {
                    ret.Append(new string(' ',(BlockLength - ascii.Length)*3));
                }
                ret.Append("; ");
                ret.Append(ascii);
                startAddress += (ulong)BlockLength;

            }

            return ret.ToString();
        }

        private string getHexAddress(ulong address)
        {
        	return string.Format("{0:X}h: ", address).PadLeft(11, '0');
        }

        private string byteToHex(byte b)
        {
            return _hexChars[b / 16].ToString() + _hexChars[b % 16].ToString();
        }

    }
}
