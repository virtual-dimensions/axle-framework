﻿using System;
using System.Globalization;


namespace Axle.Extensions.DateTime
{
    using DateTime = System.DateTime;

    /// <summary>
    /// A static class that contains extension methods for the <see cref="DateTime"/> struct
    /// </summary>
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// Returns the number of days in the specified month and year.
        /// </summary>
        /// <param name="current">
        /// The <see cref="System.DateTime"/> instance upon which this extension method is invoked.
        /// </param>
        /// <returns>
        /// Returns the number of days in the specified date's month and year. 
        /// For example, if month equals 2 for February, the return value is 28 or 29 depending upon whether year is a 
        /// leap year. 
        /// </returns>
        /// <seealso cref="System.DateTime.DaysInMonth" />
        public static int DaysInMonth(this DateTime current)
        {
            return DateTime.DaysInMonth(current.Year, current.Month);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the first day of the same week as the <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value from the same week to get the first day of week from.
        /// </param>
        /// <param name="culture">
        /// A <see cref="CultureInfo"/> instance representing the cutlure to obtain date-time settings from.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the first day of the same week as the <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="FirstDayOfWeek(System.DateTime, System.DayOfWeek)"/>
        /// <seealso cref="LastDayOfWeek(System.DateTime, System.Globalization.CultureInfo)"/>
        public static DateTime FirstDayOfWeek(this DateTime current, CultureInfo culture)
        {
            return FirstDayOfWeek(current, culture.DateTimeFormat.FirstDayOfWeek);
        }
        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the first day of the same week as the <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value from the same week to get the first day of week from.
        /// </param>
        /// <param name="firstDayOfWeek">
        /// A <see cref="DayOfWeek"/> value telling which day of the week is considered the first.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the first day of the same week as the <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="FirstDayOfWeek(System.DateTime, System.Globalization.CultureInfo)"/>
        /// <seealso cref="LastDayOfWeek(System.DateTime, System.DayOfWeek)"/>
        public static DateTime FirstDayOfWeek(this DateTime current, DayOfWeek firstDayOfWeek)
        {
            var currentDayOfWeek = current.DayOfWeek;
            if (firstDayOfWeek == currentDayOfWeek)
            {
                return current.Date;
            }

            var result = current.Date;
            var fdw = (int) firstDayOfWeek;
            for (var i = (int) currentDayOfWeek; i <= fdw; i--)
            {
                result = result.AddDays(-1);
            }
            return result;
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the first day of the same month as <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value in the same month to get the first day from.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the first day of the same month as <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="LastDayOfMonth"/>
        public static DateTime FirstDayOfMonth(this DateTime current)
        {
            return new DateTime(current.Year, current.Month, 1, 0, 0, 0, current.Kind);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the first day of the same year as <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value in the same year to get the first day from.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the first day of the same year as <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="LastDayOfYear"/>
        public static DateTime FirstDayOfYear(this DateTime current)
        {
            return new DateTime(current.Year, 1, 1, 0, 0, 0, current.Kind);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the last day of the same week as the <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value from the same week to get the last day of week from.
        /// </param>
        /// <param name="culture">
        /// A <see cref="CultureInfo"/> instance representing the cutlure to obtain date-time settings from.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the last day of the same week as the <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="FirstDayOfWeek(System.DateTime, System.DayOfWeek)"/>
        /// <seealso cref="LastDayOfWeek(System.DateTime, System.Globalization.CultureInfo)"/>
        public static DateTime LastDayOfWeek(this DateTime current, CultureInfo culture)
        {
            return LastDayOfWeek(current, culture.DateTimeFormat.FirstDayOfWeek);
        }
        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the last day of the same week as the <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value from the same week to get the last day of week from.
        /// </param>
        /// <param name="firstDayOfWeek">
        /// A <see cref="DayOfWeek"/> value telling which day of the week is considered the first.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the last day of the same week as the <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="FirstDayOfWeek(System.DateTime, System.Globalization.CultureInfo)"/>
        /// <seealso cref="LastDayOfWeek(System.DateTime, System.DayOfWeek)"/>
        public static DateTime LastDayOfWeek(this DateTime current, DayOfWeek firstDayOfWeek)
        {
            return FirstDayOfWeek(current, firstDayOfWeek).AddDays(6);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the last day of the same month as <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value in the same month to get the last day from.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the last day of the same month as <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="FirstDayOfMonth"/>
        public static DateTime LastDayOfMonth(this DateTime current)
        {
            return new DateTime(current.Year, current.Month, DaysInMonth(current), 0, 0, 0, 0, current.Kind);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value representing the last day of the same year as <paramref name="current"/> date time is in.
        /// </summary>
        /// <param name="current">
        /// A <see cref="DateTime"/> value in the same year to get the last day from.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value representing the last day of the same year as <paramref name="current"/> date time is in.
        /// </returns>
        /// <seealso cref="FirstDayOfYear"/>
        public static DateTime LastDayOfYear(this DateTime current)
        {
            return new DateTime(current.Year, 12, 31, 0, 0, 0, current.Kind);
        }


        /// <summary>
        /// Returns a new <see cref="DateTime"/> value with the same number of <see cref="DateTime.Ticks"/> but with 
        /// a <see cref="DateTime.Kind"/> value as specified by the <paramref name="kind"/> parameter.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> value to change the kind of.</param>
        /// <param name="kind">
        /// The <see cref="DateTimeKind"/> value to use for the changed <see cref="DateTime"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="DateTime"/> value with the same number of <see cref="DateTime.Ticks"/> but with 
        /// a <see cref="DateTime.Kind"/> value as specified by the <paramref name="kind"/> parameter.
        /// </returns>
        /// <seealso cref="ChangeKindToLocal"/>
        /// <seealso cref="ChangeKindToUtc"/>
        /// <seealso cref="DateTime.Kind"/>
        /// <seealso cref="DateTimeKind"/>
        public static DateTime ChangeKind(this DateTime dateTime, DateTimeKind kind) { return new DateTime(dateTime.Ticks, kind); }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value with the same number of <see cref="DateTime.Ticks"/> but with 
        /// a <see cref="DateTime.Kind"/> value changed to <see cref="DateTimeKind.Local"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> value to change the kind of.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> value with the same number of <see cref="DateTime.Ticks"/> but with 
        /// a <see cref="DateTime.Kind"/> value changed to <see cref="DateTimeKind.Local"/>.
        /// </returns>
        /// <seealso cref="ChangeKindToLocal"/>
        /// <seealso cref="ChangeKindToUtc"/>
        /// <seealso cref="DateTime.Kind"/>
        /// <seealso cref="DateTimeKind.Local"/>
        public static DateTime ChangeKindToLocal(this DateTime dateTime) { return ChangeKind(dateTime, DateTimeKind.Local); }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value with the same number of <see cref="DateTime.Ticks"/> but with 
        /// a <see cref="DateTime.Kind"/> value changed to <see cref="DateTimeKind.Utc"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> value to change the kind of.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> value with the same number of <see cref="DateTime.Ticks"/> but with 
        /// a <see cref="DateTime.Kind"/> value changed to <see cref="DateTimeKind.Utc"/>.
        /// </returns>
        /// <seealso cref="ChangeKindToLocal"/>
        /// <seealso cref="ChangeKindToUtc"/>
        /// <seealso cref="DateTime.Kind"/>
        /// <seealso cref="DateTimeKind.Utc"/>
        public static DateTime ChangeKindToUtc(this DateTime dateTime) { return ChangeKind(dateTime, DateTimeKind.Utc); }
    }
}