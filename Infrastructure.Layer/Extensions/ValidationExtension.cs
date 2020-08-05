using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Layer.Extensions
{
    [DebuggerNonUserCode()]
    public static class ValidationExtension
    {
        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        public static bool HasValue(this Guid value)
        {
            return !value.IsEmpty();
        }

        public static bool HasValue(this object o)
        {
            return o != null;
        }

        public static bool HasValue(this string value)
        {
            return !value.IsNullOrEmpty();
        }

        public static bool HasValue(this int value)
        {
            return value > 0;
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static bool HasValue<T>(this IEnumerable<T> valueList)
        {
            return valueList != null && valueList.Any();
        }

        public static bool HasValue<T>(this IList<T> valueList)
        {
            return !valueList.IsNullOrEmpty();
        }

        public static bool IsNullOrEmpty<T>(this IList<T> objectLists)
        {
            return objectLists == null || objectLists.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this byte[] objectLists)
        {
            return objectLists == null || objectLists.Length == 0;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty(this int? value)
        {
            return value == null || value == 0;
        }

        public static bool IsEmpty(this int value)
        {
            return value == 0;
        }

        public static bool IsDouble(this string strDouble)
        {
            if (strDouble.IsNullOrEmpty())
            {
                return false;
            }

            try
            {
                Convert.ToDouble(strDouble);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool IsOnlyInteger(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var objRegex = new Regex("[^0-9]");

            return !objRegex.IsMatch(value);
        }

        public static bool IsAlphaNumeric(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var objRegex = new Regex("[^a-zA-Z0-9]");

            return !objRegex.IsMatch(value);
        }

        public static bool IsValidAlphaNumericWithSpace(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var objAlphaNumericPattern = new Regex("[^a-zA-Z0-9\\s]");

            return !objAlphaNumericPattern.IsMatch(value);
        }

        public static bool IsAlpha(this string value)
        {
            try
            {
                if (value.IsNullOrEmpty())
                {
                    return false;
                }

                var objRegex = new Regex("[^a-zA-Z]");

                return !objRegex.IsMatch(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsDecimal(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                Convert.ToDecimal(value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNumber(this string value)
        {
            try
            {
                Convert.ToDouble(value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInteger(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                Convert.ToInt32(value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTime(this string value)
        {
            try
            {
                Convert.ToDateTime(value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTimeDefault(this DateTime value)
        {
            try
            {
                return value == Convert.ToDateTime("01/01/1900 00:00:00");
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTimeMin(this DateTime value)
        {
            try
            {
                return value == Convert.ToDateTime("01/01/0001 00:00:00");
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEmail(this string email)
        {
            return Regex.Match(email, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$").Success;
        }

        public static bool IsPhoneDdd(this int phoneDdd)
        {
            return phoneDdd > 0 && phoneDdd < 99;
        }

        public static bool IsPhoneNumber(this string phoneNumber)
        {
            return phoneNumber.Length == 9 || phoneNumber.Length == 8;
        }

        public static bool IsCpf(this string cpf)
        {
            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var strTempCpf = default(string);
            var strDigito = default(string);
            var intSoma = default(int);
            var intResto = default(int);

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
            {
                return false;
            }

            strTempCpf = cpf.Substring(0, 9);

            intSoma = 0;

            for (var i = 0; i < 9; i++)
            {
                intSoma += int.Parse(strTempCpf[i].ToString()) * multiplicador1[i];
            }

            intResto = intSoma % 11;

            if (intResto < 2)
            {
                intResto = 0;
            }
            else
            {
                intResto = 11 - intResto;
            }

            strDigito = intResto.ToString();

            strTempCpf = strTempCpf + strDigito;

            intSoma = 0;

            for (var i = 0; i < 10; i++)
            {
                intSoma += int.Parse(strTempCpf[i].ToString()) * multiplicador2[i];
            }

            intResto = intSoma % 11;

            if (intResto < 2)
            {
                intResto = 0;
            }
            else
            {
                intResto = 11 - intResto;
            }

            strDigito = strDigito + intResto.ToString();

            return cpf.EndsWith(strDigito);
        }

        public static bool IsCnpj(this string cnpj)
        {
            var strCNPJLimpo = cnpj.Replace(".", "");

            strCNPJLimpo = strCNPJLimpo.Replace("/", "");
            strCNPJLimpo = strCNPJLimpo.Replace("-", "");

            if (cnpj.Length != 14)
            {
                return false;
            }

            var digitos = default(int[]);
            var soma = default(int[]);
            var resultado = default(int[]);
            var nrDig = default(int);
            var ftmt = default(string);
            var CNPJOk = default(bool[]);

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            for (nrDig = 0; nrDig < 14; nrDig++)
            {
                digitos[nrDig] = int.Parse(strCNPJLimpo.Substring(nrDig, 1));

                if (nrDig <= 11)
                {
                    soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
                }

                if (nrDig <= 12)
                {
                    soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }

            }

            for (nrDig = 0; nrDig < 2; nrDig++)
            {
                resultado[nrDig] = (soma[nrDig] % 11);

                if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                {
                    CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                }
                else
                {
                    CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }
            }

            return (CNPJOk[0] && CNPJOk[1]);
        }

        public static bool IsCpjCnpj(this string cpfCnpj)
        {
            return (cpfCnpj.IsCpf() || cpfCnpj.IsCnpj());
        }

        public static bool IsGenericList(this object objectValue)
        {
            var typeObject = objectValue.GetType();

            return (typeObject.IsGenericType && (typeObject.GetGenericTypeDefinition() == typeof(List<>)));
        }

        public static bool IsEquals(this string value, string valueEquals)
        {
            return value.Trim().ToUpper().Equals(valueEquals.Trim().ToUpper());
        }

        public static bool IsContains(this string value, string valueEquals)
        {
            return value.Trim().ToUpper().Contains(valueEquals.Trim().ToUpper());
        }

        public static bool HasValue(this Stream stream)
        {
            return (stream == null || stream.Length == 0);
        }

        public static bool HasLowerChar(this string value)
        {
            return new Regex(@"[a-z]+").IsMatch(value);
        }

        public static bool HasUpperChar(this string value)
        {
            return new Regex(@"[A-Z]+").IsMatch(value);
        }

        public static bool HasNumber(this string value)
        {
            return new Regex(@"[0-9]+").IsMatch(value);
        }

        public static bool HasSymbols(this string value)
        {
            return new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(value);
        }
    }
}
