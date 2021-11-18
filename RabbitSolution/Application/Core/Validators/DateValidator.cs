using System;

namespace Application.Core.Validators
{
    public static class DateValidator
    {
        public static bool Valid(string date)
        {
            return string.IsNullOrEmpty(date) || (date.ToDate() < DateTime.Today && date.ToDate() > DateTime.MinValue);
        }
    }
}
