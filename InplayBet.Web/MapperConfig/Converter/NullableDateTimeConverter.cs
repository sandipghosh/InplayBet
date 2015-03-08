

namespace InplayBet.Web.MapperConfig.Converter
{
    using AutoMapper;
    using System;

    public class NullableDateTimeConverter : TypeConverter<DateTime?, DateTime?>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override DateTime? ConvertCore(DateTime? source)
        {
            return source;
        }
    }
}