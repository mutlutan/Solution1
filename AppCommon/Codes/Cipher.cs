using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon
{
    public class MyCipher
    {
        //String Convert Type
        public enum EnmSCType
        {
            None,
            Base64,
            Hex
        }

        #region private

        private static string ToStr(string s)
        {
            return s == null ? "" : s.ToString();
        }

        private static string EncryptOrDecrypt(string _str, string _key)
        {
            var result = new StringBuilder();

            for (int i = 0; i < _str.Length; i++)
            {
                // take next character from string
                char character = _str[i];

                // cast to a uint
                uint charCode = character;

                // figure out which character to take from the key
                int keyPosition = i % _key.Length; // use modulo to "wrap round"

                // take the key character
                char keyChar = _key[keyPosition];

                // cast it to a uint also
                uint keyCode = keyChar;

                // perform XOR on the two character codes
                uint combinedCode = charCode ^ keyCode;

                // cast back to a char
                char combinedChar = (char)combinedCode;

                // add to the result
                result.Append(combinedChar);
            }

            return result.ToString();
        }

        #endregion

        public static string Encrypt(string text, string key, EnmSCType enmSCType, Encoding encoding)
        {
            text = ToStr(text);
            key = ToStr(key);

            text = EncryptOrDecrypt(text, key);

            if (enmSCType == EnmSCType.Base64)
            {
                text = Convert.ToBase64String(encoding.GetBytes(text));
            }

            if (enmSCType == EnmSCType.Hex)
            {
                text = text.MyStrToHex(encoding);
            }

            return text;
        }

        public static string Decrypt(string text, string key, EnmSCType enmSCType, Encoding encoding)
        {
            text = ToStr(text);
            key = ToStr(key);

            if (enmSCType == EnmSCType.Base64)
            {
                text = encoding.GetString(Convert.FromBase64String(text));
            }

            if (enmSCType == EnmSCType.Hex)
            {
                text = text.MyHexToStr(encoding);
            }

            text = EncryptOrDecrypt(text, key);

            return text;
        }


    }

}
