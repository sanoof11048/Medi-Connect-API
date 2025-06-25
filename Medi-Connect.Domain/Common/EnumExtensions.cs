using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Common
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? enumValue.ToString();
        }

        public static string[] GetFlagsDisplayNames<TEnum>(this TEnum value) where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(flag => value.HasFlag(flag) && Convert.ToInt32(flag) != 0)
                .Select(flag => (flag as Enum)!.GetDisplayName())
                .ToArray();
        }

    }

}
