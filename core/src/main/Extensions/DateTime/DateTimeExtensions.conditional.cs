﻿using System;


namespace Axle.Extensions.DateTime
{
    using DateTime = System.DateTime;

    
    public static partial class DateTimeExtensions
    {
        #if !NETSTANDARD || NETSTANDARD1_5_OR_NEWER
        /// <summary>
        /// Converts the given <see cref="DateTime"/> value to local date time.
        /// </summary>
        /// <param name="current">
        /// The <see cref="DateTime"/> value to convet.
        /// </param>
        /// <param name="assumedKind">
        /// A <see cref="DateTimeKind"/> value to be used as the assumed <see cref="DateTime.Kind">kind</see> of the 
        /// <paramref name="current">given</paramref> <see cref="DateTime"/> in case its kind was set to <see cref="DateTimeKind.Unspecified"/>
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value that represents <paramref name="current">a given</paramref> <see cref="DateTime"/> value into the local <see cref="TimeZone"/>.
        /// </returns>
        /// <seealso cref="DateTime.Kind"/>
        /// <seealso cref="DateTimeKind"/>
        /// <seealso cref="TimeZone"/>
        /// <seealso cref="ToLocalTime(System.DateTime)"/>
        /// <seealso cref="ChangeTimeZone(System.DateTime,System.TimeZoneInfo,System.TimeZoneInfo)"/>
        /// <seealso cref="TimeZoneInfo.ConvertTime(System.DateTimeOffset,System.TimeZoneInfo)"/>
        public static DateTime ToLocalTime(this DateTime current, DateTimeKind assumedKind)
        {
            var kind = current.Kind;
            return (kind == DateTimeKind.Unspecified ? assumedKind : kind) == DateTimeKind.Utc
                ? TimeZoneInfo.ConvertTime(current, TimeZoneInfo.Utc, TimeZoneInfo.Local)
                : new DateTime(current.Ticks, DateTimeKind.Local);
        }
        /// <summary>
        /// Converts the given <see cref="DateTime"/> value to local date time.
        /// <remarks>
        /// In case the <paramref name="current"/> <see cref="DateTime.Kind"/> property is set to <see cref="DateTimeKind.Unspecified"/>,
        /// it is assumed that the date is a local date (as if it were <see cref="DateTimeKind.Local"/>).
        /// </remarks>
        /// </summary>
        /// <param name="current">
        /// The <see cref="DateTime"/> value to convet.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value that represents <paramref name="current">a given</paramref> <see cref="DateTime"/> value into the local <see cref="TimeZone"/>.
        /// </returns>
        /// <seealso cref="DateTime.Kind"/>
        /// <seealso cref="DateTimeKind"/>
        /// <seealso cref="TimeZone"/>
        /// <seealso cref="ToLocalTime(System.DateTime,DateTimeKind)"/>
        /// <seealso cref="ChangeTimeZone(System.DateTime,System.TimeZoneInfo,System.TimeZoneInfo)"/>
        /// <seealso cref="TimeZoneInfo.ConvertTime(System.DateTimeOffset,System.TimeZoneInfo)"/>
        public static DateTime ToLocalTime(this DateTime current) { return ToLocalTime(current, DateTimeKind.Local); }

        /// <summary>
        /// Converts a time from one time zone to another. 
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime"/> instance upon which this extension method is invoked.
        /// </param>
        /// <param name="sourceTimeZone">
        /// The time zone of the given <paramref name="dateTime"/>. 
        /// </param>
        /// <param name="destinationTimeZone">
        /// The time zone to convert <paramref name="dateTime"/> to.
        /// </param>
        /// <param name="assumeLocal">
        /// A boolean value indicating whether the provided <see cref="DateTime"/> value in the
        /// <paramref name="dateTime"/> should be treated as a local time in the <paramref name="sourceTimeZone"/>
        /// if its <see cref="DateTime.Kind" /> property is set to <see cref="DateTimeKind.Unspecified"/>.
        /// If this value is <c>false</c>, the <paramref name="dateTime"/> is assumed to be an UTC date, otherwise
        /// it is treated as a date local to the timezone specified by the <paramref name="sourceTimeZone"/>.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value that represents the date and time in the destination time zone which 
        /// corresponds to the <paramref name="dateTime"/> parameter in the source time zone. 
        /// </returns>
        public static DateTime ChangeTimeZone(
            this DateTime dateTime,
            TimeZoneInfo sourceTimeZone,
            TimeZoneInfo destinationTimeZone,
            bool assumeLocal)
        {
            var utcTimeZone = TimeZoneInfo.Utc;
            var sourceIsUtc = utcTimeZone.Equals(sourceTimeZone);
            var destinationIsUtc = utcTimeZone.Equals(destinationTimeZone);
            if (sourceIsUtc && destinationIsUtc)
            {
                return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
            }
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                if (!assumeLocal && !sourceIsUtc)
                {
                    dateTime = TimeZoneInfo.ConvertTime(dateTime, utcTimeZone, sourceTimeZone);
                }
            }
            else if (dateTime.Kind == DateTimeKind.Utc)
            {
                if (!sourceIsUtc)
                {
                    dateTime = TimeZoneInfo.ConvertTime(dateTime, utcTimeZone, sourceTimeZone);
                }
            }
            return new DateTime(
                TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destinationTimeZone).Ticks,
                destinationIsUtc ? DateTimeKind.Utc : DateTimeKind.Unspecified);
        }
        /// <summary>
        /// Converts a time from one time zone to another. 
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime"/> instance upon which this extension method is invoked.
        /// </param>
        /// <param name="sourceTimeZone">
        /// The time zone of the given <paramref name="dateTime"/>. 
        /// </param>
        /// <param name="destinationTimeZone">
        /// The time zone to convert <paramref name="dateTime"/> to.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value that represents the date and time in the destination time zone which 
        /// corresponds to the <paramref name="dateTime"/> parameter in the source time zone. 
        /// </returns>
        public static DateTime ChangeTimeZone(
            this DateTime dateTime,
            TimeZoneInfo sourceTimeZone,
            TimeZoneInfo destinationTimeZone)
        {
            return ChangeTimeZone(dateTime, sourceTimeZone, destinationTimeZone, false);
        }

        /// <summary>
        /// Converts a time to the universal time zone (UTC).
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime"/> instance upon which this extension method is invoked.
        /// </param>
        /// <param name="sourceTimeZone">
        /// The time zone of the given <paramref name="dateTime"/>. 
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value that represents the date and time in the UTC time zone which 
        /// corresponds to the <paramref name="dateTime"/> parameter. 
        /// </returns>
        public static DateTime ToUniversalTime(this DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            return ChangeTimeZone(dateTime, sourceTimeZone, TimeZoneInfo.Utc, true);
        }
        #endif
    }
}