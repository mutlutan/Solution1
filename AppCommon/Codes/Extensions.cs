using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

#nullable disable

namespace AppCommon
{
    #region Genel Extensions

    public static class Extensions
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
            return _value == null ? "" : _value.ToString();
        }

        public static string MyToTrim(this object _value)
        {
            return _value == null ? "" : _value.ToString().Trim();
        }

        public static string MyToStrAntiInjection(this string _str)
        {
            string rV = "";
            if (!string.IsNullOrEmpty(_str))
            {
                rV = _str.Replace("+", "").Replace("'", "").Replace("-", "").Replace("\"", "").Replace(",",""); //virgülde çıkartılmalı
            }

            return rV;
        }

        public static string MyToMaxLength(this string _str, int _len)
        {
            if (!string.IsNullOrEmpty(_str))
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

            if (!string.IsNullOrEmpty(_str))
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

        public static string MyToSeoUrl(this string _str)
        {
            _str = _str.MyToLower();
            _str = _str.MyToTrim();
            _str = _str.Replace("ğ", "g");
            _str = _str.Replace("Ğ", "G");
            _str = _str.Replace("ü", "u");
            _str = _str.Replace("Ü", "U");
            _str = _str.Replace("ş", "s");
            _str = _str.Replace("Ş", "S");
            _str = _str.Replace("ı", "i");
            _str = _str.Replace("İ", "I");
            _str = _str.Replace("ö", "o");
            _str = _str.Replace("Ö", "O");
            _str = _str.Replace("ç", "c");
            _str = _str.Replace("Ç", "C");
            _str = _str.Replace("-", "+");
            _str = _str.Replace(" ", "+");
            _str = _str.Trim();
            _str = new Regex("[^a-zA-Z0-9+]").Replace(_str, "");
            _str = _str.Trim();
            _str = _str.Replace("+", "-");

            return _str;
        }

        public static string MyToLower(this string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToLower();
            }
            return _str;
        }

        public static string MyToUpper(this string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToUpper();
            }
            return _str;
        }

        public static string MyToTitleCase(this string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                _str = _str.Trim();
                if (_str.Length > 0)
                {
                    //_str = System.Globalization.CultureInfo.DefaultThreadCurrentCulture.TextInfo.ToTitleCase(_str);
                    //dnx core TextInfo.ToTitleCase i destekleyince aşağıyı silip üstekini kullan
                    string[] words = _str.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].Length == 0) continue;

                        char firstChar = char.ToUpper(words[i][0]);
                        string rest = "";
                        if (words[i].Length > 1)
                        {
                            rest = words[i][1..].ToLower();
                        }
                        words[i] = firstChar + rest;
                    }
                    _str = string.Join(" ", words);
                }
            }
            return _str;
        }

        public static string MyStrToFirstStar(this string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                _str = _str.Trim();
                if (_str.Length > 0)
                {

                    string[] words = _str.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].Length == 0) continue;

                        char firstChar = words[i][0];
                        string rest = "";
                        if (words[i].Length > 1)
                        {
                            rest = "".PadRight(words[i][1..].Length, '*');
                        }
                        words[i] = firstChar + rest;
                    }
                    _str = string.Join(" ", words);
                }
            }
            return _str;
        }

        public static string MyMoonToStr(this string _str)
        {
            //01-12 ye kadar olan harf olarak döner
            string rValue = "X";
            try
            {
                char[] chars = { 'X', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'K', 'L', 'M', 'N' };

                if (!string.IsNullOrEmpty(_str))
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

        /// <summary>
        /// a-z, A-Z, 0-9 or _
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string MyToCleanStr(this string _str)
        {
            return Regex.Replace(_str.MyToTrim(), "[^A-Za-z0-9 -]", "");
        }
        #endregion

        #region ad soyad split 
        public static string MyToSurname(this string _nameSurname)
        {
            return _nameSurname.Split(' ').LastOrDefault();
        }
        public static string MyToName(this string _nameSurname)
        {
            return _nameSurname.Replace(_nameSurname.MyToSurname(), "");
        }
        #endregion

        #region xml
        public static string MyObjectToXmlStr<T>(this T value)
        {
            //kullanım >>> var xmlString = obj.Serialize();
            string rV = "";

            if (value == null)
            {
                rV = string.Empty;
            }
            try
            {
                var xmlserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using var writer = System.Xml.XmlWriter.Create(stringWriter);
                xmlserializer.Serialize(writer, value);
                rV = stringWriter.ToString();

            }
            catch { }

            return rV;
        }
        #endregion

        #region int
        public static int MyToInt(this string _str)
        {
            int rValue = 0;
            if (!string.IsNullOrEmpty(_str))
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


        public static DateTime YearToStartOfYearDate(this int year)
        {
            return new DateTime(year, 1, 1).Date;
        }
        public static DateTime YearToEndOfYearDate(this int year)
        {
            return new DateTime(year, 12, 31).Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime YearToStartOfDateWhitPeriod(this int year, int period)
        {
            return year.YearToStartOrEndOfDateWhitPeriod(period, true);
        }
        public static DateTime YearToEndOfDateWhitPeriod(this int year, int period)
        {
            return year.YearToStartOrEndOfDateWhitPeriod(period, false);
        }
        private static DateTime YearToStartOrEndOfDateWhitPeriod(this int year, int period, bool isStart)
        {
            int startMonth;
            int endMonth;
            switch (period)
            {
                case 2:
                    startMonth = 4;
                    endMonth = 6;
                    break;
                case 3:
                    startMonth = 7;
                    endMonth = 9;
                    break;
                case 4:
                    startMonth = 10;
                    endMonth = 12;
                    break;
                default:
                    startMonth = 1;
                    endMonth = 3;
                    break;
            }
            return isStart ? new DateTime(year, startMonth, 1) : new DateTime(year, endMonth + 1, 1).Date.AddTicks(-1);
        }
        #endregion

        #region decimal
        public static decimal MyToDecimal(this string _str)
        {
            decimal rValue = 0;
            if (!string.IsNullOrEmpty(_str))
            {
                _ = decimal.TryParse(_str, out rValue);
            }
            return rValue;
        }

        public static decimal MyToDecimal(this object _value)
        {
            decimal rValue = 0;
            if (_value != null)
            {
                try
                {
                    rValue = Convert.ToDecimal(_value);
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
            roman += thou[num / 1000 % 10];
            roman += hun[num / 100 % 10];
            roman += ten[num / 10 % 10];
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

        #region Cryptography
        public static string MyGetMD5HashFromFile(this string _fileName)
        {
            string rV = "";
            if (!string.IsNullOrEmpty(_fileName))
            {
                if (File.Exists(_fileName))
                {
                    using var md5 = MD5.Create();
                    using var stream = File.OpenRead(_fileName);
                    rV = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            return rV;
        }

        public static string MyToMD5(this byte[] _byteArray)
        {
            string rV = "";
            if (_byteArray != null)
            {
                MD5 MD5Pass = MD5.Create();
                byte[] MD5Buff = MD5Pass.ComputeHash(_byteArray);
                rV = BitConverter.ToString(MD5Buff).Replace("-", string.Empty);
            }
            return rV;
        }

        public static string MyToMD5(this string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                MD5 MD5Pass = MD5.Create();
                byte[] MD5Buff = MD5Pass.ComputeHash(Encoding.UTF8.GetBytes(_str));
                _str = BitConverter.ToString(MD5Buff).Replace("-", string.Empty);
            }
            return _str;
        }

        public static string MyToBase64Str(this string _str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(_str.MyToStr()));
        }

        public static string MyFromBase64Str(this string _str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(_str.MyToStr()));
        }

        public static string MyToEncrypt(this string _str, string _key, MyCipher.EnmSCType enmSCType, Encoding encoding)
        {
            return MyCipher.Encrypt(_str.MyToStr(), _key.MyToStr(), enmSCType, encoding);
        }

        public static string MyToDecrypt(this string _str, string _key, MyCipher.EnmSCType enmSCType, Encoding encoding)
        {
            return MyCipher.Decrypt(_str.MyToStr(), _key.MyToStr(), enmSCType, encoding);
        }

        public static string MyToEncrypt(this string _str, string _key)
        {
            return MyCipher.Encrypt(_str.MyToStr(), _key, MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        public static string MyToDecrypt(this string _str, string _key)
        {
            return MyCipher.Decrypt(_str.MyToStr(), _key, MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        public static string MyToEncryptPassword(this string _str)
        {
            return MyCipher.Encrypt(_str.MyToStr(), "6856", MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        public static string MyToDecryptPassword(this string _str)
        {
            return MyCipher.Decrypt(_str.MyToStr(), "6856", MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        #endregion

        #region Hex
        public static string MyStrToHex(this object _value, Encoding _encoding)
        {
            var sb = new StringBuilder();

            var bytes = _encoding.GetBytes(_value.MyToStr());
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string MyHexToStr(this object _value, Encoding _encoding)
        {
            string rValue = _value.MyToStr();

            var bytes = new byte[rValue.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(rValue.Substring(i * 2, 2), 16);
            }

            rValue = _encoding.GetString(bytes);

            return rValue;
        }
        #endregion

        #region crom ex
        //public static string MyToCronExpressionDescriptor(this string _str, string _language)
        //{
        //    string rV = _str;
        //    try
        //    {
        //        //Install-Package CronExpressionDescriptor
        //        //rV = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(
        //        //    _str.MyToTrim(), 
        //        //    new CronExpressionDescriptor.Options() { Locale = _language }
        //        //);
        //        rV = _str;
        //    }
        //    catch { }
        //    return rV;
        //}
        #endregion

        #region enum ex
        public static string MyGetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (_Attribs != null && _Attribs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }


        public static string MyGetDisplayName(this Enum enumValue)
        {
            string displayName;
            displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = enumValue.ToString();
            }

            return displayName;
        }

        #endregion 

        #region Validasyon
        public static bool IsValidTCKimlikNo(this string kimlikNo)
        {
            int Algoritma_Adim_Kontrol = 0, TekBasamaklarToplami = 0, CiftBasamaklarToplami = 0;

            if (kimlikNo.Length == 11) Algoritma_Adim_Kontrol = 1;
            foreach (char chr in kimlikNo) { if (char.IsNumber(chr)) Algoritma_Adim_Kontrol = 2; }
            if (kimlikNo[..1] != "0") Algoritma_Adim_Kontrol = 3;

            int[] arrTC = Regex.Replace(kimlikNo, "[^0-9]", "").Select(x => (int)char.GetNumericValue(x)).ToArray();

            for (int i = 0; i < kimlikNo.Length; i++)
            {
                if ((i + 1) % 2 == 0)
                    if (i + 1 != 10) CiftBasamaklarToplami += Convert.ToInt32(arrTC[i]);
                    else
                    if (i + 1 != 11) TekBasamaklarToplami += Convert.ToInt32(arrTC[i]);
            }

            if (Convert.ToInt32(kimlikNo.Substring(9, 1)) == (TekBasamaklarToplami * 7 - CiftBasamaklarToplami) % 10) Algoritma_Adim_Kontrol = 4;
            if (Convert.ToInt32(kimlikNo.Substring(10, 1)) == (arrTC.Sum() - Convert.ToInt32(kimlikNo.Substring(10, 1))) % 10) Algoritma_Adim_Kontrol = 5;

            return Algoritma_Adim_Kontrol == 5;
        }
        public static bool IsValidEmail(this string mailAdres)
        {
            return new EmailAddressAttribute().IsValid(mailAdres);
        }
        #endregion

        #region String i bir dönüşüme sokar, mail için kullanılacak. 
        public static string MyStrToMD5ByteSumHex(this string _value)
        {
            int values = 0;
            Encoding encoding = Encoding.UTF8;
            _value = _value.MyToMD5();
            //byte sum
            var bytes = encoding.GetBytes(_value);
            foreach (int t in bytes)
            {
                values += t;
            }
            return values.MyStrToHex(encoding).ToString();
        }
        #endregion

        #region Object To JsonText
        public static string MyObjToJsonText(this object _obj)
        {
            string rV = "";
            if (_obj != null)
            {
                rV = JsonSerializer.Serialize(_obj);// JsonConvert.SerializeObject(_obj);
            }
            return rV;
        }
        #endregion


        #region Json String To Object


        public static T JsonToObject<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        #endregion

        #region Veri dili için 
        public static string MyVeriDiliToStr(this string _jsonText, string _lang, string _field, string _filedCurrentValue)
        {
            if (_lang == "tr")
            {
                return _filedCurrentValue;
            }
            string rValue = _filedCurrentValue.MyToTrim();
            try
            {
                if (!string.IsNullOrEmpty(_jsonText))
                {
                    var obj = JsonNode.Parse(_jsonText).AsObject();
                    //dynamic data = System.Text.Json.JsonSerializer.Deserialize<dynamic>(_jsonText);//  Newtonsoft.Json.Linq.JObject.Parse(_jsonText);
                    if (obj[_field] != null)
                    {
                        if (obj[_field][_lang] != null)
                        {
                            rValue = obj[_field][_lang].ToString();
                        }
                    }
                }
            }
            catch { }

            return rValue;
        }
        #endregion

    }

    #endregion

    #region EnumerableExtensions
    public static class EnumerableExtensions
    {
        public static IEnumerable<IterationElement<T>> MyToDetailed<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using var enumerator = source.GetEnumerator();
            bool isFirst = true;
            bool hasNext = enumerator.MoveNext();
            int index = 0;
            while (hasNext)
            {
                T current = enumerator.Current;
                hasNext = enumerator.MoveNext();
                yield return new IterationElement<T>(index, current, isFirst, !hasNext);
                isFirst = false;
                index++;
            }
        }

        public readonly struct IterationElement<T>
        {
            public int Index { get; }
            public bool IsFirst { get; }
            public bool IsLast { get; }
            public T Value { get; }

            public IterationElement(int index, T value, bool isFirst, bool isLast)
            {
                Index = index;
                IsFirst = isFirst;
                IsLast = isLast;
                Value = value;
            }
        }
    }

    #endregion
        
  
    #region object clone DeepCopy  
    //public static T MyClone<T>(this T self)
    //{
    //    var serialized = JsonConvert.SerializeObject(self);
    //    return JsonConvert.DeserializeObject<T>(serialized);
    //}
    #endregion

}
