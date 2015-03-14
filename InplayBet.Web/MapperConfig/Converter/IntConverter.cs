
namespace InplayBet.Web.MapperConfig.Converter
{
    using AutoMapper;
    using System;

    public class IntConverter : TypeConverter<int?, int>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override int ConvertCore(int? source)
        {
            if (source.HasValue)
                return source.Value;
            else
                return default(int);
        }
    }

    public class LongConverter : TypeConverter<long?, long>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override long ConvertCore(long? source)
        {
            if (source.HasValue)
                return source.Value;
            else
                return default(long);
        }
    }

    public class DecimalConverter : TypeConverter<decimal?, decimal>
    {
        /// <summary>
        /// Converts the core.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override decimal ConvertCore(decimal? source)
        {
            if (source.HasValue)
                return source.Value;
            else
                return default(decimal);
        }
    }
}