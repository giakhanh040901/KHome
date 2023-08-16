using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace EPIC.Utils.EnumType
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the type of the attribute of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumVal">The enum value.</param>
        /// <returns></returns>

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }
        private static EnumMemberAttribute GetEnumMemberAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                return null;
            }

            var field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<EnumMemberAttribute>();
        }

        public static string ToEnumMemberString(this Enum enu)
        {
            var attr = GetEnumMemberAttribute(enu);
            return attr != null ? attr.Value : enu.ToString();
        }

        public static string ToEnumString(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                return null;
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<DisplayAttribute>();
        }

        public static string ToEnumDescriptionString(this Enum enu)
        {
            var attr = GetDescriptionAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DescriptionAttribute GetDescriptionAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                return null;
            }

            var field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<DescriptionAttribute>();
        }

        // convert enume value to list
        // em dùng hàm này để hiện thị danh sách status trong phần chuyển trạng thái nhé value là id và display là name
        public static List<EnumDto> GetList<T>() where T : Enum
        {
            List<EnumDto> enums = ((T[])Enum.GetValues(typeof(T))).Select(c => new EnumDto()
            {
                Value = ToEnumMemberString(c),
                Name = ToEnumDescriptionString(c)
            }).ToList();
            return enums;
        }

        public static int GetEnumFromDescription(string description, Type enumType)
        {
            foreach (var field in enumType.GetFields())
            {
                DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null)
                    continue;
                if (attribute.Description == description)
                {
                    return (int)field.GetValue(null);
                }
            }
            return 0;
        }
    }
}
