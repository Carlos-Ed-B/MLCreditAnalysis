using Infrastructure.Layer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Layer.Extensions
{
    public static class HelperExtension
    {
        #region Enum
        public static List<T> ToEnumList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static bool IsInEnumDescription<T>(string text)
        {
            var enuns = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            foreach (var item in enuns)
            {
                if (text.ToUpper().Equals(item.ToDescription().ToUpper())) { return true; }
            }

            return false;
        }

        public static bool IsInEnum<T>(this T source)
        {
            var enuns = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            return enuns.Any(x => x.DefaultInteger() == source.DefaultInteger());
        }

        public static string ToDescription<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());

            if (fi == null) { return string.Empty; }

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static List<EnumModel> GetEnumList<T>()
        {
            var array = (T[])(Enum.GetValues(typeof(T)).Cast<T>());

            return array
                          .Select(a => new EnumModel
                          {
                              Key = a.ToString(),
                              Nome = a.ToString(),
                              Descricao = a.ToDescription(),
                              Id = Convert.ToInt32(a)
                          }).OrderBy(kvp => kvp.Nome).ToList();
        }

        public static T GetEnumBy<T>(this string value)
        {
            var array = (T[])(Enum.GetValues(typeof(T)).Cast<T>());
            return array.FirstOrDefault(x => x.ToString().IsEquals(value));

        }

        private static T[] ToArray<T>()
        {
            return (T[])Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static List<int> GetIdList<T>()
        {
            T[] array = HelperExtension.ToArray<T>();

            return array.Select(a => Convert.ToInt32(a)).ToList();
        }

        public static List<string> GetStringList<T>()
        {
            T[] array = HelperExtension.ToArray<T>();

            return array.Select(a => Convert.ToString(a)).ToList();
        }

        public static List<int> ToIntList<T>(this List<T> list) where T : Enum
        {
            var ids = new List<int>();

            foreach (var item in list)
            {
                ids.Add(item.ToInteger());
            }

            return ids;
        }

        #endregion

        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void Merge<T>(this T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }

        public static bool ComparerPropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
                return self == to;

            Type type = typeof(T);
            List<string> ignoreList = new List<string>(ignore);

            foreach (System.Reflection.PropertyInfo pi in type.GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (!ignoreList.Contains(pi.Name))
                    continue;

                object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Return a list of properties that have a different values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="to"></param>
        /// <param name="ignore"> aaa </param>
        /// <returns>
        /// [
        ///     {
        ///         Item1 = "NameProperty",
        ///         Item2 = oldValue,
        ///         Item3 = newValue
        ///     }
        /// ]
        /// </returns>
        public static List<Tuple<string, object, object>> DiffValueProrerties<T>(this T self, T to, params string[] ignore) where T : class
        {
            var result = new List<Tuple<string, object, object>>();

            if (self == null || to == null)
                return result;

            Type type = typeof(T);
            List<string> ignoreList = new List<string>(ignore);

            foreach (System.Reflection.PropertyInfo pi in type.GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (ignoreList.Contains(pi.Name))
                    continue;

                object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    result.Add(new Tuple<string, object, object>(pi.Name, selfValue, toValue));
            }

            return result;
        }

        #region IEnumerable

        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource>(this IEnumerable<TSource> source, string sortColumn, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var orderBy = default(Func<TSource, object>);
            var resultType = typeof(TSource);

            if (resultType.GetProperties().Any(prop => prop.Name == sortColumn))
            {
                var propertyInfo = resultType.GetProperty(sortColumn);
                orderBy = (data => propertyInfo.GetValue(data, null));
            }

            return sortDirection == ListSortDirection.Descending ?
                   source.OrderByDescending(orderBy) : source.OrderBy(orderBy);
        }

        #endregion

        #region String

        public static string ReplaceMultiSpace(this string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }

        public static string Left(this string stringValue, int size)
        {
            if (stringValue.Length < size) { size = stringValue.Length; }

            return stringValue.Substring(0, size);
        }

        public static string Right(this string stringValue, int size)
        {
            return stringValue.Substring(stringValue.Length - size, size);
        }

        public static string Mid(this string stringValue, int indexBegin, int indexEnd)
        {
            return stringValue.Substring(indexBegin, indexEnd);
        }

        public static string Mid(this string stringValue, int size)
        {
            return stringValue.Substring(size);
        }

        public static bool HasSpecialChar(this string input)
        {
            Regex r = new Regex(
              "(?:[^a-zA-Z0-9 -]|(?<=['\"])s)",
              RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.IsMatch(input);
        }

        public static string GetTextBetween(this string textComplet, string firstString, string lastString)
        {
            if (textComplet.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var positionFirst = textComplet.IndexOf(firstString) + firstString.Length;
            var positionLast = textComplet.IndexOf(lastString);

            if (positionFirst <= 0 || positionLast <= 0)
            {
                return string.Empty;
            }

            var finalText = textComplet.Substring(positionFirst, positionLast - positionFirst);

            return finalText.Trim();
        }

        public static string ToEncodeUTF8(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            var textFormated = string.Empty;

            //execuÃ§Ã£o
            text = text.Replace("saÃ­da", "saída");
            text = text.Replace("ÃƒO", "ão");
            text = text.Replace("Ã£o", "ão");
            text = text.Replace("Ã‡", "Ç");
            text = text.Replace("Ã§", "ç");
            text = text.Replace("possÃ­vel", "possí­vel");
            text = text.Replace("TransferÃªncia­vel", "Transferência");

            if (HelperExtension.HasEncodeInText(text))
            {
                var textSplit = text.Split(' ');

                foreach (var textTemp in textSplit)
                {
                    var textEncoded = textTemp;
                    if (HelperExtension.HasEncodeInText(textEncoded))
                    {
                        textEncoded = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(textEncoded.DefaultString()));
                    }

                    textFormated += $" {textEncoded}";
                }
            }
            else
            {
                return text;
            }

            return textFormated.Trim();
        }

        private static bool HasEncodeInText(string text)
        {
            return (text.Contains("£") || text.Contains("Ã³") || text.Contains("Ã£") || text.Contains("Ã©") || text.Contains("Ã£") || text.Contains("¡") || text.Contains("Ãª") || text.Contains("Ã§"));
        }

        #endregion

        #region Linq
        /// <summary>
        /// Retorna uma lista unica de objetos pelos parametros informados
        /// Exemplo: 
        ///     var result = pessoaLista.DistinctBy(p => p.Id)
        ///     var result = pessoaLista.DistinctBy(p => new { p.Id, p.Documento })
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize, bool resetInit = false)
        {
            if (resetInit)
            {

                return source.Take((page + 1) * pageSize);
            }
            return source.Skip(page * pageSize).Take(pageSize);
        }

        public static IList<int> NotIn(this IList<int> sourceList, IList<int> newList)
        {
            return (from c in sourceList
                    where !(from o in newList
                            select o)
                          .Contains(c)
                    select c).ToList();
        }

        public static bool In<T>(this T source, IList<T> list)
        {
            return list.Contains(source);
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource objectDefault)
        {
            TSource result = source.FirstOrDefault();

            if (result == null)
            {
                return objectDefault;
            }

            return result;
        }
        #endregion

        public static string GetTableName(this Type type)
        {
            var tableScherma = (TableAttribute[])type.GetCustomAttributes(typeof(TableAttribute), false);

            return tableScherma.Length > 0 ? tableScherma[0].Name : type.Name;
        }

        public static int GetDateDiffInDays(this DateTime dateBegin, DateTime dateEnd)
        {
            return (dateBegin - dateEnd).TotalDays.ToInteger();
        }

        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

    }
}
