
namespace InplayBet.Web.Utilities.Expression
{
    using System.ComponentModel.DataAnnotations;
    public static class BinaryOperator
    {
        /// <summary>
        /// Gets the equals to.
        /// </summary>
        /// <value>
        /// The equals to.
        /// </value>
        [Display(Name = "Equals To")]
        public static string EqualsTo { get { return "{0}={1}"; } }

        /// <summary>
        /// Gets the not equals to.
        /// </summary>
        /// <value>
        /// The not equals to.
        /// </value>
        [Display(Name = "Not Equals To")]
        public static string NotEqualsTo { get { return "{0}<>{1}"; } }

        /// <summary>
        /// Gets the less than.
        /// </summary>
        /// <value>
        /// The less than.
        /// </value>
        [Display(Name = "Less Than")]
        public static string LessThan { get { return "{0}<{1}"; } }

        /// <summary>
        /// Gets the less than equal.
        /// </summary>
        /// <value>
        /// The less than equal.
        /// </value>
        [Display(Name = "Less Than Equal")]
        public static string LessThanEqual { get { return "{0}<={1}"; } }

        /// <summary>
        /// Gets the greater than.
        /// </summary>
        /// <value>
        /// The greater than.
        /// </value>
        [Display(Name = "Greater Than")]
        public static string GreaterThan { get { return "{0}>{1}"; } }

        /// <summary>
        /// Gets the greater than equal.
        /// </summary>
        /// <value>
        /// The greater than equal.
        /// </value>
        [Display(Name = "Greater Than Equal")]
        public static string GreaterThanEqual { get { return "{0}>={1}"; } }

        /// <summary>
        /// Gets the contains.
        /// </summary>
        /// <value>
        /// The contains.
        /// </value>
        [Display(Name = "Contains")]
        public static string Contains { get { return "{0}.ContainsSearchEx({1})"; } }

        /// <summary>
        /// Gets the starts with.
        /// </summary>
        /// <value>
        /// The starts with.
        /// </value>
        [Display(Name = "Starts With")]
        public static string StartsWith { get { return "{0}.StartsWithSearchEx({1})"; } }

        /// <summary>
        /// Gets the ends with.
        /// </summary>
        /// <value>
        /// The ends with.
        /// </value>
        [Display(Name = "Ends With")]
        public static string EndsWith { get { return "{0}.EndsWithSearchEx({1})"; } }
    }
}