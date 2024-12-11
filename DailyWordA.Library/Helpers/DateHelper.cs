using System;

namespace DailyWordA.Library.Helpers;

public static class DateHelper
{
    /// <summary>
    /// 将 DateTime 转换为 ISO 8601 格式的字符串 (yyyy-MM-dd)
    /// </summary>
    /// <param name="date">要转换的 DateTime 对象</param>
    /// <returns>格式化的字符串</returns>
    public static string ToDateString(DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 将 ISO 8601 格式的字符串转换为 DateTime
    /// </summary>
    /// <param name="dateString">ISO 8601 格式的字符串</param>
    /// <returns>转换后的 DateTime 对象</returns>
    public static DateTime FromIso8601DateString(string dateString)
    {
        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None,
                out var result))
        {
            return result;
        }

        throw new FormatException($"无法将字符串 '{dateString}' 转换为有效的 DateTime 对象。");
    }
}