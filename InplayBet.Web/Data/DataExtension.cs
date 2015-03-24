

namespace InplayBet.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.Utilities;

    public static class DataExtension
    {
        public static IQueryable<T> GetPaggedData<T>(this IQueryable<T> data, int pageIndex, int pageCount)
            where T : BaseData
        {
            return data.Skip(pageCount * (pageIndex - 1)).Take(pageCount);
        }
        public static IEnumerable<T> GetPaggedDataCompiled<T>(this IEnumerable<T> data, int pageIndex, int pageCount)
            where T : BaseData
        {
            return data.Skip(pageCount * (pageIndex - 1)).Take(pageCount);
        }
    }
    public static class QueryableExtensions
    {
        public static ProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
    }

    public class ProjectionExpression<TSource>
    {
        private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();

        private readonly IQueryable<TSource> _source;

        public ProjectionExpression(IQueryable<TSource> source)
        {
            _source = source;
        }

        public IQueryable<TDest> To<TDest>()
        {
            try
            {
                var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();
                return _source.Select(queryExpression);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            try
            {
                var key = GetCacheKey<TDest>();
                return ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDest>> : null;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            try
            {
                var sourceProperties = typeof(TSource).GetProperties();
                var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
                var parameterExpression = Expression.Parameter(typeof(TSource), "src");

                var bindings = destinationProperties
                    .Select(destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                    .Where(binding => binding != null);

                var expression = Expression.Lambda<Func<TSource, TDest>>
                    (Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

                var key = GetCacheKey<TDest>();
                ExpressionCache.Add(key, expression);
                return expression;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            try
            {
                var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

                if (sourceProperty != null)
                {
                    return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
                }

                var propertyNames = SplitCamelCase(destinationProperty.Name);

                if (propertyNames.Length == 2)
                {
                    sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == propertyNames[0]);

                    if (sourceProperty != null)
                    {
                        var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);

                        if (sourceChildProperty != null)
                        {
                            return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(parameterExpression, destinationProperty, sourceProperties);
            }
            return null;
        }

        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }

        private static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
        }
    }
}