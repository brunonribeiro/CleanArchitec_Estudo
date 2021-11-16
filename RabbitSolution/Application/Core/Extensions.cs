using System;

namespace Application.Core
{
    public static class Extensions
    {
        public static DateTime? ToDate(this string str)
        {
            var result = DateTime.TryParse(str, out var date);

            if (result)
                return date;

            return null;
        }
    }
}
