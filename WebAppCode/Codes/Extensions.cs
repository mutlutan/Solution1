using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace WebApp1.Codes
{
    #region Genel Extensions

    public static class MyExtensions
    {
        #region Exception için

        public static Exception MyLastInner(this Exception _ex)
        {
            if (_ex.InnerException == null) return _ex;
            return _ex.InnerException.MyLastInner();
        }

        #endregion

        #region string işlemler

        public static string MyToStr(this object _value)
        {
            string? rV = Convert.ToString(_value);
            return rV ?? "";
        }

        public static string MyToTrim(this object _value)
        {
            return _value.MyToStr().Trim();
        }

        public static string MyToStrAntiInjection(this string _str)
        {
            string rV = "";
            if (!string.IsNullOrEmpty(_str))
            {
                rV = _str.Replace("+", "").Replace("'", "").Replace("-", "").Replace("\"", "");
            }

            return rV;
        }

        public static string MyToMaxLength(this string _str, int _len)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                if (_str.Length > _len)
                {
                    _str = _str[.._len];
                }
            }

            return _str;
        }

        public static string MyToLatinString(this string _str)
        {
            //burası Culture e göre değişebilir olacak
            //tr-TR
            char[] chars1 = { 'Ç', 'Ğ', 'İ', 'Ş', 'Ö', 'Ü', 'Â' };
            char[] chars2 = { 'C', 'G', 'I', 'S', 'O', 'U', 'A' };

            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToUpper();

                char[] charArray = _str.ToCharArray();

                for (int i = 0; i < charArray.Length; i++)
                {
                    for (int k = 0; k < chars1.Length; k++)
                    {
                        if (charArray[i] == chars1[k])
                        {
                            charArray[i] = chars2[k];
                        }
                    }
                }

                _str = new string(charArray);
            }
            return _str;
        }

        public static string MyToLower(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToLower();
            }
            return _str;
        }

        public static string MyToUpper(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToUpper();
            }
            return _str;
        }

        public static string MyToTitleCase(this string _str)
        {
            _str = System.Globalization.CultureInfo.CurrentCulture.TextInfo?.ToTitleCase(_str ?? "") ?? "";

            return _str;
        }

        public static string MyStrToFirstStar(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim();
                if (_str.Length > 0)
                {

                    String[] words = _str.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].Length == 0) continue;

                        Char firstChar = words[i][0];
                        String rest = "";
                        if (words[i].Length > 1)
                        {
                            rest = "".PadRight(words[i][1..].Length, '*');
                        }
                        words[i] = firstChar + rest;
                    }
                    _str = String.Join(" ", words);
                }
            }
            return _str;
        }

        public static string MyMoonToStr(this string _str)
        {
            //01-12 ye kadar olan harf olarak döner
            String rValue = "X";
            try
            {
                char[] chars = { 'X', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'K', 'L', 'M', 'N' };

                if (!String.IsNullOrEmpty(_str))
                {
                    rValue = chars[_str.MyToInt()].ToString();
                }
            }
            catch { }

            return rValue;
        }

        public static string MyToReverseStr(this string _str)
        {
            char[] charArray = _str.MyToStr().ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion

        #region int
        public static int MyToInt(this string _str)
        {
            int rValue = 0;
            if (!String.IsNullOrEmpty(_str))
            {
                _ = int.TryParse(_str, out rValue);
            }
            return rValue;
        }

        public static int MyToInt(this object _value)
        {
            int rValue = 0;
            if (_value != null)
            {
                try
                {
                    rValue = Convert.ToInt32(_value);
                }
                catch { }
            }
            return rValue;
        }

        #endregion

        #region Roman
        public static string MyToRoman(this int num)
        {
            string[] thou = { "", "M", "MM", "MMM" };
            string[] hun = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] ten = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            string roman = "";
            roman += thou[(int)(num / 1000) % 10];
            roman += hun[(int)(num / 100) % 10];
            roman += ten[(int)(num / 10) % 10];
            roman += ones[num % 10];
            return roman;
        }

        public static int MyRomanToInt(this string roman)
        {
            Dictionary<char, int> RomanMap = new() { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
            int number = 0;
            char previousChar = roman[0];
            foreach (char currentChar in roman)
            {
                number += RomanMap[currentChar];
                if (RomanMap[previousChar] < RomanMap[currentChar])
                {
                    number -= RomanMap[previousChar] * 2;
                }
                previousChar = currentChar;
            }
            return number;
        }
        #endregion

    }

    #endregion

   
}
