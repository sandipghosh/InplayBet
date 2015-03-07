
namespace InplayBet.Web.Utilities.Expression
{
    using System;

    /// <summary>
    /// Basic type of exception for this engine. 
    /// Important difference is Position property it provides: vaue is index of character in string expression on which problem was detected.
    /// </summary>
    public class RequestEngineException : ApplicationException
    {
        int position;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position
        {
            get { return position; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestEngineException"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="message">The message.</param>
        internal RequestEngineException(int position, string message = "") : base(message) { this.position = position; }
    }

    /// <summary>
    /// All ExpressionParser's exceptions
    /// </summary>
    public class ParserException : RequestEngineException
    {
        public enum ParserExceptionType
        {
            QuatesNotClosed,
            BracketsNotClosed,
            InvalidBracketsOrder
        }

        ParserExceptionType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        internal ParserException(int position, ParserExceptionType type, string message = "") : base(position, message) { this.type = type; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ParserExceptionType Type { get { return type; } }
    }

    /// <summary>
    /// All ExpressionBuilder's exceptions
    /// </summary>
    public class BuilderException : RequestEngineException
    {
        public enum BuilderExceptionType
        {
            WrongArgFormat,
            ArgNumberExceedsMax,
            UnrecognizedConversionType,
            UnexpectedConstant,
            UnexpectedExpression,
            PropertyNotExists,
            FunctionNotExists,
            ParameterTypeNotSupported,
            FunctionArgumentsExpected,
            WrongArgumentsNumber,
            NoFunctionFound,
            NoLeftOperand,
            NoRightOperand,
            IncorrectUnaryOperatorPosition,
            IncorrectExpression,
            UnexpectedError
        }

        BuilderExceptionType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderException"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public BuilderException(int position, BuilderExceptionType type, string message = "") : base(position, message) { this.type = type; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public BuilderExceptionType Type { get { return type; } }
    }

    /// <summary>
    /// All exceptions related to types incompatibilities found in expression
    /// </summary>
    public class ExpressionTypeException : RequestEngineException
    {
        Type expectedType;
        Type obtainedType;

        /// <summary>
        /// Gets the expected type.
        /// </summary>
        /// <value>
        /// The expected type.
        /// </value>
        public Type ExpectedType
        {
            get { return expectedType; }
        }

        /// <summary>
        /// Gets the type of the obtained.
        /// </summary>
        /// <value>
        /// The type of the obtained.
        /// </value>
        public Type ObtainedType
        {
            get { return obtainedType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTypeException"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="obtainedType">Type of the obtained.</param>
        public ExpressionTypeException(int position, Type expectedType, Type obtainedType) : this(position, expectedType, obtainedType, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTypeException"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="obtainedType">Type of the obtained.</param>
        /// <param name="message">The message.</param>
        public ExpressionTypeException(int position, Type expectedType, Type obtainedType, string message)
            : base(position, message)
        {
            this.expectedType = expectedType;
            this.obtainedType = obtainedType;
        }
    }
}