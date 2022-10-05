using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace UnitTestProject1
{
    public static class Utilities
    {
        public static byte[] HexStringToHex(this string inputHex)
        {
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }

        public static string Data_Asc_Hex(string Data)
        {
            //first take each charcter using substring.
            //then  convert character into ascii.
            //then convert ascii value into Hex Format

            string sValue;
            string sHex = "";
            while (Data.Length > 0)
            {
                sValue = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()));
                Data = Data.Substring(1, Data.Length - 1);
                sHex = sHex + sValue;
            }
            return sHex;
        }
        public static string Data_Hex_Asc(string Data)
        {
            string Data1 = "";
            string sData = "";

            while (Data.Length > 0)

            //first take two hex value using substring.
            //then  convert Hex value into ascii.
            //then convert ascii value into character.
            {
                Data1 = System.Convert.ToChar(System.Convert.ToUInt32(Data.Substring(0, 2), 16)).ToString();
                sData = sData + Data1;
                Data = Data.Substring(2, Data.Length - 2);
            }
            return sData;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static  string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
