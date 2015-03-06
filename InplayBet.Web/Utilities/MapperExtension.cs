
namespace InplayBet.Web.Utilities
{
    #region Required Namespace
    using AutoMapper;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    #endregion;

    public static class MapperExtension
    {
        public static Expression<Func<TDestination, TResult>> RemapForType<TSource, TDestination, TResult>
            (this Expression<Func<TSource, TResult>> expression)
        {
            Contract.Requires(expression != null);
            Contract.Ensures(Contract.Result<Expression<Func<TDestination, TResult>>>() != null);

            var newParameter = System.Linq.Expressions.Expression.Parameter(typeof(TDestination));

            Contract.Assume(newParameter != null);
            var visitor = new AutoMapVisitor<TSource, TDestination>(newParameter);
            var remappedBody = visitor.Visit(expression.Body);
            if (remappedBody == null)
                throw new InvalidOperationException("Unable to remap expression");

            return System.Linq.Expressions.Expression.Lambda<Func<TDestination, TResult>>(remappedBody, newParameter);
        }

        /// <summary>
        /// Ignores all non existing.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType)
                && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }

        /// <summary>
        /// Bothes the ways.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="mappingExpression">The mapping expression.</param>
        /// <returns></returns>
        public static IMappingExpression<TDestination, TSource> MapBothWays<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            return Mapper.CreateMap<TDestination, TSource>();
        }
    }

    public class AutoMapVisitor<TSource, TDestination> : ExpressionVisitor
    {
        private readonly ParameterExpression _newParameter;
        private readonly TypeMap _typeMap = Mapper.FindTypeMapFor<TSource, TDestination>();

        /// <summary>
        /// Initialises a new instance of the <see cref="AutoMapVisitor{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="newParameter">The new <see cref="ParameterExpression"/> to access.</param>
        public AutoMapVisitor(ParameterExpression newParameter)
        {
            Contract.Requires(newParameter != null);

            _newParameter = newParameter;
            Contract.Assume(_typeMap != null);
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(_typeMap != null);
            Contract.Invariant(_newParameter != null);
        }

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MemberExpression"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var propertyMaps = _typeMap.GetPropertyMaps();
            Contract.Assume(propertyMaps != null);

            // Find any mapping for this member
            var propertyMap = propertyMaps.SingleOrDefault(map => map.SourceMember == node.Member);
            if (propertyMap == null)
                return base.VisitMember(node);

            var destinationProperty = propertyMap.DestinationProperty;

            Contract.Assume(destinationProperty != null);
            var destinationMember = destinationProperty.MemberInfo;

            Contract.Assume(destinationMember != null);

            // Check the new member is a property too
            var property = destinationMember as PropertyInfo;
            if (property == null)
                return base.VisitMember(node);

            // Access the new property
            var newPropertyAccess = System.Linq.Expressions.Expression.Property(_newParameter, property);
            return base.VisitMember(newPropertyAccess);
        }
    }

    static class FunctionCompositionExtensions
    {
        /// <summary>
        /// Composes the specified outer.
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <typeparam name="Z"></typeparam>
        /// <param name="outer">The outer.</param>
        /// <param name="inner">The inner.</param>
        /// <returns></returns>
        public static Expression<Func<X, Y>> Compose<X, Y, Z>(this Expression<Func<Z, Y>> outer, Expression<Func<X, Z>> inner)
        {
            return System.Linq.Expressions.Expression.Lambda<Func<X, Y>>(
                ParameterReplacer.Replace(outer.Body, outer.Parameters[0], inner.Body),
                inner.Parameters[0]);
        }
    }

    class ParameterReplacer : ExpressionVisitor
    {
        private ParameterExpression _parameter;
        private System.Linq.Expressions.Expression _replacement;

        /// <summary>
        /// Prevents a default instance of the <see cref="ParameterReplacer"/> class from being created.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="replacement">The replacement.</param>
        private ParameterReplacer(ParameterExpression parameter, System.Linq.Expressions.Expression replacement)
        {
            _parameter = parameter;
            _replacement = replacement;
        }

        /// <summary>
        /// Replaces the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression Replace
            (System.Linq.Expressions.Expression expression,
            ParameterExpression parameter, System.Linq.Expressions.Expression replacement)
        {
            return new ParameterReplacer(parameter, replacement).Visit(expression);
        }

        /// <summary>
        /// Visits the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitParameter(ParameterExpression parameter)
        {
            if (parameter == _parameter)
            {
                return _replacement;
            }
            return base.VisitParameter(parameter);
        }
    }
}