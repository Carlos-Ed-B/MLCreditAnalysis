using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Infrastructure.Layer.Extensions
{
    [DebuggerNonUserCode()]
    public static class DefaultExtension
    {

        public static string DefaultString(this IList<string> stringLista, int index, string valorRetorno = "")
        {
            if (stringLista.Count > index)
            {
                return stringLista[index];
            }

            return valorRetorno;

        }

        public static string DefaultString(this string strValor, string strValorRetorno)
        {
            if (strValor.IsNullOrEmpty())
            {
                return strValorRetorno;
            }

            return strValor.Trim();
        }

        public static string DefaultString(this string strValor)
        {
            return strValor.DefaultString(string.Empty);
        }

        public static string DefaultString(this object objValor, string strValorRetorno)
        {
            if (objValor == null || objValor is DBNull)
            {
                return strValorRetorno;
            }

            return objValor.ToString().DefaultString(strValorRetorno);
        }

        public static string DefaultString(this object objValor)
        {
            if (objValor == null || objValor is DBNull)
            {
                return string.Empty;
            }

            return objValor.ToString().DefaultString();
        }

        public static string Default(this string strValor)
        {
            return strValor.DefaultString(string.Empty);
        }

        public static int DefaultInteger(this string strValor, int intValorRetorno)
        {
            if (strValor.IsNullOrEmpty())
            {
                return intValorRetorno;
            }

            return strValor.To<int>();
        }

        public static int DefaultInteger(this string strValor)
        {
            return strValor.DefaultInteger(0);
        }

        public static int DefaultInteger(this object objValor, int intValorRetorno = 0)
        {
            if (objValor == null || objValor is DBNull || objValor.DefaultString().IsNullOrEmpty())
            {
                return intValorRetorno;
            }

            return objValor.To<int>();
        }

        public static decimal DefaultDecimal(this object objValor, decimal dcmValorRetorno = 0)
        {
            if (objValor == null || objValor is DBNull || objValor.DefaultString().IsNullOrEmpty())
            {
                return dcmValorRetorno;
            }

            return objValor.To<decimal>();
        }

        public static DateTime DefaultDateTime(this string strValor, DateTime dtmValorRetorno)
        {
            if (string.IsNullOrEmpty(strValor))
            {
                return dtmValorRetorno;
            }

            return strValor.To<DateTime>();
        }

        public static DateTime DefaultDateTime(this string strValor)
        {
            return strValor.DefaultDateTime(DateTime.Parse("01/01/1901"));
        }

        public static DateTime DefaultDateTime(this object objValor, DateTime dtmValorRetorno = default(DateTime))
        {
            if (objValor == null || objValor is DBNull)
            {
                return dtmValorRetorno == default(DateTime) ? DateTime.Parse("01/01/1900") : dtmValorRetorno;
            }

            return objValor.To<DateTime>();
        }

        public static bool DefaultBool(this object objValor, bool blnValorRetorno = false)
        {
            if (objValor == null || objValor is DBNull)
            {
                return blnValorRetorno;
            }

            if (objValor.Equals("1"))
            {
                return true;
            }

            if (objValor.Equals("0"))
            {
                return false;
            }

            return objValor.To<bool>();
        }

        public static long DefaultLong(this object objValor, long lngValorRetorno = 0)
        {
            if (objValor == null || objValor is DBNull || objValor.DefaultString().IsNullOrEmpty())
            {
                return lngValorRetorno;
            }

            return objValor.To<long>();
        }

        public static long DefaultLong(this string strValor)
        {
            return strValor.DefaultLong(0);
        }

        public static List<int> DefaultIntList(this List<int> result)
        {
            if (result != null)
            {
                return result;
            }

            return new List<int>();
        }
    }
}
