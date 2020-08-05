//using System;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;

//namespace Infrastructure.Layer.Extensions
//{
//    public static class EnumExtension
//    {
//        public static TAttribute GetAttribute<TAttribute>(Enum value) where TAttribute : Attribute
//        {
//            return value.GetType().GetMember(value.ToString())[0].GetCustomAttribute<TAttribute>();
//        }

//        public static string ToDescription<T>(this T e) where T : Enum
//        {
//            Type type = e.GetType();
//            Array values = Enum.GetValues(type);

//            foreach (int val in values)
//            {
//                if (val == e.GetHashCode())
//                {
//                    var memInfo = type.GetMember(type.GetEnumName(val));
//                    var descriptionAttribute = memInfo[0]
//                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
//                        .FirstOrDefault() as DescriptionAttribute;

//                    if (descriptionAttribute != null)
//                    {
//                        return descriptionAttribute.Description;
//                    }
//                }
//            }

//            return e.ToString();
//        }
//    }
//}
