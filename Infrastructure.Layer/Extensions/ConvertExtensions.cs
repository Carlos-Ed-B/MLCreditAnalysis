using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Layer.Extensions
{
    public static class ConvertExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static long ToTimestamp(this DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this byte b)
        {
            return (T)System.Enum.ToObject(typeof(T), b);
        }

        public static T ToEnum<T>(this int b)
        {
            return (T)System.Enum.ToObject(typeof(T), b);
        }

        public static T To<T>(this object o)
        {
            if (o is IConvertible)
            {
                return (T)Convert.ChangeType(o, typeof(T));
            }

            return (T)o;
        }

        public static string ToFileName(this DateTime dateObject)
        {
            return $"{dateObject.Year}_{dateObject.Month}_{dateObject.Day}_{dateObject.Hour}_{dateObject.Minute}_{dateObject.Millisecond}";
        }

        public static object ToFormatByteParametroDB(this object objByte)
        {
            if (objByte == null || objByte.ToString().IsNullOrEmpty())
            {
                return DBNull.Value;
            }

            return objByte;
        }

        public static short ToShort(this object ObjectToConvert)
        {
            return Convert.ToInt16(ObjectToConvert);
        }

        public static short ToShort(this string strToConvert)
        {
            return Convert.ToInt16(strToConvert);
        }

        public static int ToInteger(this string strToConvert)
        {
            int.TryParse(strToConvert, out int result);

            return result;
        }

        public static int ToInteger(this object ObjectToConvert)
        {
            return Convert.ToInt32(ObjectToConvert);
        }

        public static long ToLong(this string strToConvert)
        {
            return Convert.ToInt64(strToConvert);
        }

        public static long ToLong(this object ObjectToConvert)
        {
            return Convert.ToInt64(ObjectToConvert);
        }

        public static double ToDouble(this string strValor)
        {
            return Convert.ToDouble(strValor);
        }

        public static double ToDouble(this object ObjectToConvert)
        {
            return Convert.ToDouble(ObjectToConvert.ToString());
        }

        public static decimal ToDecimal(this object ObjectToConvert)
        {
            return Convert.ToDecimal(ObjectToConvert.ToString());
        }

        public static decimal ToDecimal(this string strValor)
        {
            return Convert.ToDecimal(strValor);
        }

        public static bool ToBool(this object Object)
        {
            return Convert.ToBoolean(Object.ToString());
        }

        public static bool ToBool(this int intObject)
        {
            return intObject == 1 ? true : false;
        }

        public static DateTime ToDateTime(this string valeu)
        {
            return Convert.ToDateTime(valeu);
        }

        public static DateTime ToDateTime(this object obj)
        {
            return Convert.ToDateTime(Convert.ToString(obj));
        }

        public static string ToDateOnly(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string ToDateText(this DateTime date)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;

            string month = culture.TextInfo.ToTitleCase(dateTimeFormat.GetMonthName(date.Month));

            string data = date.Day + " de " + month + " de " + date.Year;

            return data;
        }

        public static string ToDateAndfWeekText(this DateTime date)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;

            string month = culture.TextInfo.ToTitleCase(dateTimeFormat.GetMonthName(date.Month));
            string dayOfWeek = culture.TextInfo.ToTitleCase(dateTimeFormat.GetDayName(date.DayOfWeek));

            string data = dayOfWeek + ", " + date.Day + " de " + month + " de " + date.Year;

            return data;
        }

        public static Guid ToGuid(this string value)
        {
            return new Guid(value);
        }

        public static string ToOnlyNumber(this string value)
        {
            Regex regexObj = new Regex(@"[^\d]");
            return regexObj.Replace(value, string.Empty);
        }

        public static string ToCleanDocument(this string value)
        {
            if (value.IsNullOrEmpty()) { return string.Empty; }

            return value.Replace(".", "").Replace("-", "").Replace(@"\", "");
        }

        /// <summary>
        /// Troca os acentos por letras 'normais'
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToNormalization(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return string.Empty;
            }

            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static List<int> ToIntList(this String[] strings)
        {
            return strings.Select(x => x.ToInteger()).ToList().DefaultIntList();
        }

    }
}
