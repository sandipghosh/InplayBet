

namespace InplayBet.Web.MapperConfig.Converter
{
    using AutoMapper;
    using System;

    public class NullableIntConverter : TypeConverter<int?, int?>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override int? ConvertCore(int? source)
        {
            return source;
        }
    }

    public class NullableLongConverter : TypeConverter<long?, long?>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override long? ConvertCore(long? source)
        {
            return source;
        }
    }

    public class NullableDecimalConverter : TypeConverter<decimal?, decimal?>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override decimal? ConvertCore(decimal? source)
        {
            return source;
        }
    }
}