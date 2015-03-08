
namespace InplayBet.Web.MapperConfig.Converter
{
    using AutoMapper;
    using System;

    public class DateTimeConverter : TypeConverter<DateTime?, DateTime>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override DateTime ConvertCore(DateTime? source)
        {
            if (source.HasValue)
                return source.Value;
            else
                return default(DateTime);
        }
    }
}