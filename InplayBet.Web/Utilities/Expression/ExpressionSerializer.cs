
namespace InplayBet.Web.Utilities.Expression
{
    #region Required Namespace
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Xml.Linq;
    #endregion;

    public abstract class CustomExpressionXmlConverter
    {
        /// <summary>
        /// Deserializes the specified expression XML.
        /// </summary>
        /// <param name="expressionXml">The expression XML.</param>
        /// <returns></returns>
        public abstract Expression Deserialize(XElement expressionXml);

        /// <summary>
        /// Serializes the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public abstract XElement Serialize(Expression expression);
    }

    public class ExpressionSerializer
    {
        private static readonly Type[] attributeTypes = new[] { typeof(string), typeof(int), typeof(bool), typeof(ExpressionType) };
        private Dictionary<string, ParameterExpression> parameters = new Dictionary<string, ParameterExpression>();
        private ExpressionSerializationTypeResolver resolver;
        public List<CustomExpressionXmlConverter> Converters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSerializer"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public ExpressionSerializer(ExpressionSerializationTypeResolver resolver)
        {
            this.resolver = resolver;
            Converters = new List<CustomExpressionXmlConverter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSerializer"/> class.
        /// </summary>
        public ExpressionSerializer()
        {
            this.resolver = new ExpressionSerializationTypeResolver();
            Converters = new List<CustomExpressionXmlConverter>();
        }

        /*
         * SERIALIZATION 
         */

        /// <summary>
        /// Serializes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public XElement Serialize(Expression e)
        {
            return GenerateXmlFromExpressionCore(e);
        }

        /// <summary>
        /// Generates the XML from expression core.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private XElement GenerateXmlFromExpressionCore(Expression e)
        {
            if (e == null)
                return null;
            XElement replace = ApplyCustomConverters(e);
            if (replace != null)
                return replace;
            return new XElement(
                        GetNameOfExpression(e),
                        from prop in e.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        select GenerateXmlFromProperty(prop.PropertyType, prop.Name, prop.GetValue(e, null)));
        }

        /// <summary>
        /// Applies the custom converters.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private XElement ApplyCustomConverters(Expression e)
        {
            foreach (var converter in Converters)
            {
                XElement result = converter.Serialize(e);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the name of expression.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private string GetNameOfExpression(Expression e)
        {
            if (e is LambdaExpression)
                return "LambdaExpression";
            return e.GetType().Name;
            //return XmlConvert.EncodeName(e.GetType().Name); // e.GetType().Name;
        }

        /// <summary>
        /// Generates the XML from property.
        /// </summary>
        /// <param name="propType">Type of the prop.</param>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private object GenerateXmlFromProperty(Type propType, string propName, object value)
        {
            if (attributeTypes.Contains(propType))
                return GenerateXmlFromPrimitive(propName, value);
            if (propType.Equals(typeof(object)))
                return GenerateXmlFromObject(propName, value);
            if (typeof(Expression).IsAssignableFrom(propType))
                return GenerateXmlFromExpression(propName, value as Expression);
            if (value is MethodInfo || propType.Equals(typeof(MethodInfo)))
                return GenerateXmlFromMethodInfo(propName, value as MethodInfo);
            if (value is PropertyInfo || propType.Equals(typeof(PropertyInfo)))
                return GenerateXmlFromPropertyInfo(propName, value as PropertyInfo);
            if (value is FieldInfo || propType.Equals(typeof(FieldInfo)))
                return GenerateXmlFromFieldInfo(propName, value as FieldInfo);
            if (value is ConstructorInfo || propType.Equals(typeof(ConstructorInfo)))
                return GenerateXmlFromConstructorInfo(propName, value as ConstructorInfo);
            if (propType.Equals(typeof(Type)))
                return GenerateXmlFromType(propName, value as Type);
            if (IsIEnumerableOf<Expression>(propType))
                return GenerateXmlFromExpressionList(propName, AsIEnumerableOf<Expression>(value));
            if (IsIEnumerableOf<MemberInfo>(propType))
                return GenerateXmlFromMemberInfoList(propName, AsIEnumerableOf<MemberInfo>(value));
            if (IsIEnumerableOf<ElementInit>(propType))
                return GenerateXmlFromElementInitList(propName, AsIEnumerableOf<ElementInit>(value));
            if (IsIEnumerableOf<MemberBinding>(propType))
                return GenerateXmlFromBindingList(propName, AsIEnumerableOf<MemberBinding>(value));
            throw new NotSupportedException(propName);
        }

        /// <summary>
        /// Generates the XML from object.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object GenerateXmlFromObject(string propName, object value)
        {
            object result = null;
            if (value is Type)
                result = GenerateXmlFromTypeCore((Type)value);
            if (result == null)
                result = value.ToString();
            return new XElement(propName,
                result);
        }

        /// <summary>
        /// Determines whether [is I enumerable of] [the specified prop type].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propType">Type of the prop.</param>
        /// <returns>
        ///   <c>true</c> if [is I enumerable of] [the specified prop type]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsIEnumerableOf<T>(Type propType)
        {
            if (!propType.IsGenericType)
                return false;
            Type[] typeArgs = propType.GetGenericArguments();
            if (typeArgs.Length != 1)
                return false;
            if (!typeof(T).IsAssignableFrom(typeArgs[0]))
                return false;
            if (!typeof(IEnumerable<>).MakeGenericType(typeArgs).IsAssignableFrom(propType))
                return false;
            return true;
        }

        /// <summary>
        /// Ases the I enumerable of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private IEnumerable<T> AsIEnumerableOf<T>(object value)
        {
            if (value == null)
                return null;
            return (value as IEnumerable).Cast<T>();
        }

        /// <summary>
        /// Generates the XML from element init list.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="initializers">The initializers.</param>
        /// <returns></returns>
        private object GenerateXmlFromElementInitList(string propName, IEnumerable<ElementInit> initializers)
        {
            if (initializers == null)
                initializers = new ElementInit[] { };
            return new XElement(propName,
                from elementInit in initializers
                select GenerateXmlFromElementInitializer(elementInit));
        }

        /// <summary>
        /// Generates the XML from element initializer.
        /// </summary>
        /// <param name="elementInit">The element init.</param>
        /// <returns></returns>
        private object GenerateXmlFromElementInitializer(ElementInit elementInit)
        {
            return new XElement("ElementInit",
                GenerateXmlFromMethodInfo("AddMethod", elementInit.AddMethod),
                GenerateXmlFromExpressionList("Arguments", elementInit.Arguments));
        }

        /// <summary>
        /// Generates the XML from expression list.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="expressions">The expressions.</param>
        /// <returns></returns>
        private object GenerateXmlFromExpressionList(string propName, IEnumerable<Expression> expressions)
        {
            return new XElement(propName,
                    from expression in expressions
                    select GenerateXmlFromExpressionCore(expression));
        }

        /// <summary>
        /// Generates the XML from member info list.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="members">The members.</param>
        /// <returns></returns>
        private object GenerateXmlFromMemberInfoList(string propName, IEnumerable<MemberInfo> members)
        {
            if (members == null)
                members = new MemberInfo[] { };
            return new XElement(propName,
                   from member in members
                   select GenerateXmlFromProperty(member.GetType(), "Info", member));
        }

        /// <summary>
        /// Generates the XML from binding list.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="bindings">The bindings.</param>
        /// <returns></returns>
        private object GenerateXmlFromBindingList(string propName, IEnumerable<MemberBinding> bindings)
        {
            if (bindings == null)
                bindings = new MemberBinding[] { };
            return new XElement(propName,
                from binding in bindings
                select GenerateXmlFromBinding(binding));
        }

        /// <summary>
        /// Generates the XML from binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private object GenerateXmlFromBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return GenerateXmlFromAssignment(binding as MemberAssignment);
                case MemberBindingType.ListBinding:
                    return GenerateXmlFromListBinding(binding as MemberListBinding);
                case MemberBindingType.MemberBinding:
                    return GenerateXmlFromMemberBinding(binding as MemberMemberBinding);
                default:
                    throw new NotSupportedException(string.Format("Binding type {0} not supported.", binding.BindingType));
            }
        }

        /// <summary>
        /// Generates the XML from member binding.
        /// </summary>
        /// <param name="memberMemberBinding">The member member binding.</param>
        /// <returns></returns>
        private object GenerateXmlFromMemberBinding(MemberMemberBinding memberMemberBinding)
        {
            return new XElement("MemberMemberBinding",
                GenerateXmlFromProperty(memberMemberBinding.Member.GetType(), "Member", memberMemberBinding.Member),
                GenerateXmlFromBindingList("Bindings", memberMemberBinding.Bindings));
        }

        /// <summary>
        /// Generates the XML from list binding.
        /// </summary>
        /// <param name="memberListBinding">The member list binding.</param>
        /// <returns></returns>
        private object GenerateXmlFromListBinding(MemberListBinding memberListBinding)
        {
            return new XElement("MemberListBinding",
                GenerateXmlFromProperty(memberListBinding.Member.GetType(), "Member", memberListBinding.Member),
                GenerateXmlFromProperty(memberListBinding.Initializers.GetType(), "Initializers", memberListBinding.Initializers));
        }

        /// <summary>
        /// Generates the XML from assignment.
        /// </summary>
        /// <param name="memberAssignment">The member assignment.</param>
        /// <returns></returns>
        private object GenerateXmlFromAssignment(MemberAssignment memberAssignment)
        {
            return new XElement("MemberAssignment",
                GenerateXmlFromProperty(memberAssignment.Member.GetType(), "Member", memberAssignment.Member),
                GenerateXmlFromProperty(memberAssignment.Expression.GetType(), "Expression", memberAssignment.Expression));
        }

        /// <summary>
        /// Generates the XML from expression.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private XElement GenerateXmlFromExpression(string propName, Expression e)
        {
            return new XElement(propName, GenerateXmlFromExpressionCore(e));
        }

        /// <summary>
        /// Generates the XML from type or null.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GenerateXmlFromTypeOrNull(string propName, Type type)
        {
            if (type == null) return new XElement(propName, null);
            return new XElement(propName, GenerateXmlFromTypeCore(type));
        }

        /// <summary>
        /// Generates the type of the XML from.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GenerateXmlFromType(string propName, Type type)
        {
            return new XElement(propName, GenerateXmlFromTypeCore(type));
        }

        /// <summary>
        /// Generates the XML from type core.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static XElement GenerateXmlFromTypeCore(Type type)
        {
            //vsadov: add detection of VB anon types
            if (type.Name.StartsWith("<>f__") || type.Name.StartsWith("VB$AnonymousType"))
                return new XElement("AnonymousType",
                    new XAttribute("Name", type.FullName),
                    from property in type.GetProperties()
                    select new XElement("Property",
                        new XAttribute("Name", property.Name),
                        GenerateXmlFromTypeCore(property.PropertyType)),
                    new XElement("Constructor",
                            from parameter in type.GetConstructors().First().GetParameters()
                            select new XElement("Parameter",
                                new XAttribute("Name", parameter.Name),
                                GenerateXmlFromTypeCore(parameter.ParameterType))
                    ));

            else
            {
                //vsadov: GetGenericArguments returns args for nongeneric types 
                //like arrays no need to save them.
                if (type.IsGenericType)
                {
                    return new XElement("Type",
                                            new XAttribute("Name", type.GetGenericTypeDefinition().FullName),
                                            from genArgType in type.GetGenericArguments()
                                            select GenerateXmlFromTypeCore(genArgType));
                }
                else
                {
                    return new XElement("Type", new XAttribute("Name", type.FullName));
                }

            }
        }

        /// <summary>
        /// Generates the XML from primitive.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object GenerateXmlFromPrimitive(string propName, object value)
        {
            //return new XAttribute(propName, value);
            return new XAttribute(propName, value ?? "");
        }

        /// <summary>
        /// Generates the XML from method info.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="methodInfo">The method info.</param>
        /// <returns></returns>
        private object GenerateXmlFromMethodInfo(string propName, MethodInfo methodInfo)
        {
            if (methodInfo == null)
                return new XElement(propName);
            return new XElement(propName,
                        new XAttribute("MemberType", methodInfo.MemberType),
                        new XAttribute("MethodName", methodInfo.Name),
                        GenerateXmlFromType("DeclaringType", methodInfo.DeclaringType),
                        new XElement("Parameters",
                            from param in methodInfo.GetParameters()
                            select GenerateXmlFromType("Type", param.ParameterType)),
                        new XElement("GenericArgTypes",
                            from argType in methodInfo.GetGenericArguments()
                            select GenerateXmlFromType("Type", argType)));
        }

        /// <summary>
        /// Generates the XML from property info.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        private object GenerateXmlFromPropertyInfo(string propName, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return new XElement(propName);
            return new XElement(propName,
                        new XAttribute("MemberType", propertyInfo.MemberType),
                        new XAttribute("PropertyName", propertyInfo.Name),
                        GenerateXmlFromType("DeclaringType", propertyInfo.DeclaringType),
                        new XElement("IndexParameters",
                            from param in propertyInfo.GetIndexParameters()
                            select GenerateXmlFromType("Type", param.ParameterType)));
        }

        /// <summary>
        /// Generates the XML from field info.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="fieldInfo">The field info.</param>
        /// <returns></returns>
        private object GenerateXmlFromFieldInfo(string propName, FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return new XElement(propName);
            return new XElement(propName,
                        new XAttribute("MemberType", fieldInfo.MemberType),
                        new XAttribute("FieldName", fieldInfo.Name),
                        GenerateXmlFromType("DeclaringType", fieldInfo.DeclaringType));
        }

        /// <summary>
        /// Generates the XML from constructor info.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="constructorInfo">The constructor info.</param>
        /// <returns></returns>
        private object GenerateXmlFromConstructorInfo(string propName, ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                return new XElement(propName);
            return new XElement(propName,
                        new XAttribute("MemberType", constructorInfo.MemberType),
                        new XAttribute("MethodName", constructorInfo.Name),
                        GenerateXmlFromType("DeclaringType", constructorInfo.DeclaringType),
                        new XElement("Parameters",
                            from param in constructorInfo.GetParameters()
                            select new XElement("Parameter",
                                new XAttribute("Name", param.Name),
                                GenerateXmlFromType("Type", param.ParameterType))));
        }

        /*
         * DESERIALIZATION 
         */

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public Expression Deserialize(XElement xml)
        {
            parameters.Clear();
            return ParseExpressionFromXmlNonNull(xml);
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">xml must represent an Expression<TDelegate></exception>
        public Expression<TDelegate> Deserialize<TDelegate>(XElement xml)
        {
            Expression e = Deserialize(xml);
            if (e is Expression<TDelegate>)
                return e as Expression<TDelegate>;
            throw new Exception("xml must represent an Expression<TDelegate>");
        }

        /// <summary>
        /// Determines whether [is empty XML] [the specified XML].
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>
        ///   <c>true</c> if [is empty XML] [the specified XML]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEmptyXml(XElement xml)
        {
            return xml.IsEmpty || !xml.HasElements &&
                   String.IsNullOrEmpty(xml.Value) && !xml.HasAttributes;
        }

        /// <summary>
        /// Parses the expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseExpressionFromXml(XElement xml)
        {
            if (IsEmptyXml(xml)) return null;
            return ParseExpressionFromXmlNonNull(xml.Elements().First());
        }

        /// <summary>
        /// Parses the expression from XML non null.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private Expression ParseExpressionFromXmlNonNull(XElement xml)
        {
            Expression expression = ApplyCustomDeserializers(xml);
            if (expression != null)
                return expression;
            switch (xml.Name.LocalName)
            {
                case "BinaryExpression":
                case "SimpleBinaryExpression":
                case "LogicalBinaryExpression":
                case "MethodBinaryExpression":
                    return ParseBinaryExpresssionFromXml(xml);
                case "ConstantExpression":
                case "TypedConstantExpression":
                    return ParseConstatExpressionFromXml(xml);
                case "ParameterExpression":
                case "PrimitiveParameterExpression_x0060_1":
                case "TypedParameterExpression":
                    return ParseParameterExpressionFromXml(xml);
                case "LambdaExpression":
                    return ParseLambdaExpressionFromXml(xml);
                case "MethodCallExpression":
                case "MethodCallExpressionN":
                case "InstanceMethodCallExpressionN":
                    return ParseMethodCallExpressionFromXml(xml);
                case "UnaryExpression":
                    return ParseUnaryExpressionFromXml(xml);
                case "MemberExpression":
                case "PropertyExpression":
                case "FieldExpression":
                    return ParseMemberExpressionFromXml(xml);
                case "NewExpression":
                    return ParseNewExpressionFromXml(xml);
                case "ListInitExpression":
                    return ParseListInitExpressionFromXml(xml);
                case "MemberInitExpression":
                    return ParseMemberInitExpressionFromXml(xml);
                case "ConditionalExpression":
                case "FullConditionalExpression":
                    return ParseConditionalExpressionFromXml(xml);
                case "NewArrayExpression":
                case "NewArrayInitExpression":
                case "NewArrayBoundsExpression":
                    return ParseNewArrayExpressionFromXml(xml);
                case "TypeBinaryExpression":
                    return ParseTypeBinaryExpressionFromXml(xml);
                case "InvocationExpression":
                    return ParseInvocationExpressionFromXml(xml);
                default:
                    throw new NotSupportedException(xml.Name.LocalName);
            }
        }

        /// <summary>
        /// Applies the custom deserializers.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ApplyCustomDeserializers(XElement xml)
        {
            foreach (var converter in Converters)
            {
                Expression result = converter.Deserialize(xml);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Parses the invocation expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseInvocationExpressionFromXml(XElement xml)
        {
            Expression expression = ParseExpressionFromXml(xml.Element("Expression"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments");
            return Expression.Invoke(expression, arguments);
        }

        /// <summary>
        /// Parses the type binary expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseTypeBinaryExpressionFromXml(XElement xml)
        {
            Expression expression = ParseExpressionFromXml(xml.Element("Expression"));
            Type typeOperand = ParseTypeFromXml(xml.Element("TypeOperand"), resolver);
            return Expression.TypeIs(expression, typeOperand);
        }

        /// <summary>
        /// Parses the new array expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Expected array type
        /// or
        /// Expected NewArrayInit or NewArrayBounds
        /// </exception>
        private Expression ParseNewArrayExpressionFromXml(XElement xml)
        {
            Type type = ParseTypeFromXml(xml.Element("Type"), resolver);
            if (!type.IsArray)
                throw new Exception("Expected array type");
            Type elemType = type.GetElementType();
            var expressions = ParseExpressionListFromXml<Expression>(xml, "Expressions");
            switch (xml.Attribute("NodeType").Value)
            {
                case "NewArrayInit":
                    return Expression.NewArrayInit(elemType, expressions);
                case "NewArrayBounds":
                    return Expression.NewArrayBounds(elemType, expressions);
                default:
                    throw new Exception("Expected NewArrayInit or NewArrayBounds");
            }
        }

        /// <summary>
        /// Parses the conditional expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseConditionalExpressionFromXml(XElement xml)
        {
            Expression test = ParseExpressionFromXml(xml.Element("Test"));
            Expression ifTrue = ParseExpressionFromXml(xml.Element("IfTrue"));
            Expression ifFalse = ParseExpressionFromXml(xml.Element("IfFalse"));
            return Expression.Condition(test, ifTrue, ifFalse);
        }

        /// <summary>
        /// Parses the member init expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseMemberInitExpressionFromXml(XElement xml)
        {
            NewExpression newExpression = ParseNewExpressionFromXml(xml.Element("NewExpression").Element("NewExpression")) as NewExpression;
            var bindings = ParseBindingListFromXml(xml, "Bindings").ToArray();
            return Expression.MemberInit(newExpression, bindings);
        }

        /// <summary>
        /// Parses the list init expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expceted a NewExpression</exception>
        private Expression ParseListInitExpressionFromXml(XElement xml)
        {
            NewExpression newExpression = ParseExpressionFromXml(xml.Element("NewExpression")) as NewExpression;
            if (newExpression == null) throw new Exception("Expceted a NewExpression");
            var initializers = ParseElementInitListFromXml(xml, "Initializers").ToArray();
            return Expression.ListInit(newExpression, initializers);
        }

        /// <summary>
        /// Parses the new expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseNewExpressionFromXml(XElement xml)
        {
            ConstructorInfo constructor = ParseConstructorInfoFromXml(xml.Element("Constructor"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray();
            var members = ParseMemberInfoListFromXml<MemberInfo>(xml, "Members").ToArray();
            if (members.Length == 0)
                return Expression.New(constructor, arguments);
            return Expression.New(constructor, arguments, members);
        }

        /// <summary>
        /// Parses the member expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseMemberExpressionFromXml(XElement xml)
        {
            Expression expression = ParseExpressionFromXml(xml.Element("Expression"));
            MemberInfo member = ParseMemberInfoFromXml(xml.Element("Member"));
            return Expression.MakeMemberAccess(expression, member);
        }

        /// <summary>
        /// Parses the member info from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private MemberInfo ParseMemberInfoFromXml(XElement xml)
        {
            MemberTypes memberType = (MemberTypes)ParseConstantFromAttribute<MemberTypes>(xml, "MemberType");
            switch (memberType)
            {
                case MemberTypes.Field:
                    return ParseFieldInfoFromXml(xml);
                case MemberTypes.Property:
                    return ParsePropertyInfoFromXml(xml);
                case MemberTypes.Method:
                    return ParseMethodInfoFromXml(xml);
                case MemberTypes.Constructor:
                    return ParseConstructorInfoFromXml(xml);
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotSupportedException(string.Format("MEmberType {0} not supported", memberType));
            }

        }

        /// <summary>
        /// Parses the field info from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private MemberInfo ParseFieldInfoFromXml(XElement xml)
        {
            string fieldName = (string)ParseConstantFromAttribute<string>(xml, "FieldName");
            Type declaringType = ParseTypeFromXml(xml.Element("DeclaringType"), resolver);
            return declaringType.GetField(fieldName);
        }

        /// <summary>
        /// Parses the property info from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private MemberInfo ParsePropertyInfoFromXml(XElement xml)
        {
            string propertyName = (string)ParseConstantFromAttribute<string>(xml, "PropertyName");
            Type declaringType = ParseTypeFromXml(xml.Element("DeclaringType"), resolver);
            var ps = from paramXml in xml.Element("IndexParameters").Elements()
                     select ParseTypeFromXml(paramXml, resolver);
            return declaringType.GetProperty(propertyName, ps.ToArray());
        }

        /// <summary>
        /// Parses the unary expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseUnaryExpressionFromXml(XElement xml)
        {
            Expression operand = ParseExpressionFromXml(xml.Element("Operand"));
            MethodInfo method = ParseMethodInfoFromXml(xml.Element("Method"));
            var isLifted = (bool)ParseConstantFromAttribute<bool>(xml, "IsLifted");
            var isLiftedToNull = (bool)ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
            var expressionType = (ExpressionType)ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");
            var type = ParseTypeFromXml(xml.Element("Type"), resolver);
            // TODO: Why can't we use IsLifted and IsLiftedToNull here?  
            // May need to special case a nodeType if it needs them.
            return Expression.MakeUnary(expressionType, operand, type, method);
        }

        /// <summary>
        /// Parses the method call expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseMethodCallExpressionFromXml(XElement xml)
        {
            Expression instance = ParseExpressionFromXml(xml.Element("Object"));
            MethodInfo method = ParseMethodInfoFromXml(xml.Element("Method"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray();
            return Expression.Call(instance, method, arguments);
        }

        /// <summary>
        /// Parses the lambda expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseLambdaExpressionFromXml(XElement xml)
        {
            var body = ParseExpressionFromXml(xml.Element("Body"));
            var parameters = ParseExpressionListFromXml<ParameterExpression>(xml, "Parameters");
            var type = ParseTypeFromXml(xml.Element("Type"), resolver);
            // We may need to 
            //var lambdaExpressionReturnType = type.GetMethod("Invoke").ReturnType;
            //if (lambdaExpressionReturnType.IsArray)
            //{

            //    type = typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
            //}
            return Expression.Lambda(type, body, parameters);
        }

        /// <summary>
        /// Parses the expression list from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="elemName">Name of the elem.</param>
        /// <returns></returns>
        private IEnumerable<T> ParseExpressionListFromXml<T>(XElement xml, string elemName) where T : Expression
        {
            return from tXml in xml.Element(elemName).Elements()
                   select (T)ParseExpressionFromXmlNonNull(tXml);
        }

        /// <summary>
        /// Parses the member info list from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="elemName">Name of the elem.</param>
        /// <returns></returns>
        private IEnumerable<T> ParseMemberInfoListFromXml<T>(XElement xml, string elemName) where T : MemberInfo
        {
            return from tXml in xml.Element(elemName).Elements()
                   select (T)ParseMemberInfoFromXml(tXml);
        }

        /// <summary>
        /// Parses the element init list from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="elemName">Name of the elem.</param>
        /// <returns></returns>
        private IEnumerable<ElementInit> ParseElementInitListFromXml(XElement xml, string elemName)
        {
            return from tXml in xml.Element(elemName).Elements()
                   select ParseElementInitFromXml(tXml);
        }

        /// <summary>
        /// Parses the element init from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private ElementInit ParseElementInitFromXml(XElement xml)
        {
            MethodInfo addMethod = ParseMethodInfoFromXml(xml.Element("AddMethod"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments");
            return Expression.ElementInit(addMethod, arguments);

        }

        /// <summary>
        /// Parses the binding list from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="elemName">Name of the elem.</param>
        /// <returns></returns>
        private IEnumerable<MemberBinding> ParseBindingListFromXml(XElement xml, string elemName)
        {
            return from tXml in xml.Element(elemName).Elements()
                   select ParseBindingFromXml(tXml);
        }

        /// <summary>
        /// Parses the binding from XML.
        /// </summary>
        /// <param name="tXml">The t XML.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private MemberBinding ParseBindingFromXml(XElement tXml)
        {
            MemberInfo member = ParseMemberInfoFromXml(tXml.Element("Member"));
            switch (tXml.Name.LocalName)
            {
                case "MemberAssignment":
                    Expression expression = ParseExpressionFromXml(tXml.Element("Expression"));
                    return Expression.Bind(member, expression);
                case "MemberMemberBinding":
                    var bindings = ParseBindingListFromXml(tXml, "Bindings");
                    return Expression.MemberBind(member, bindings);
                case "MemberListBinding":
                    var initializers = ParseElementInitListFromXml(tXml, "Initializers");
                    return Expression.ListBind(member, initializers);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses the parameter expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseParameterExpressionFromXml(XElement xml)
        {
            Type type = ParseTypeFromXml(xml.Element("Type"), resolver);
            string name = (string)ParseConstantFromAttribute<string>(xml, "Name");
            //vs: hack
            string id = name + type.FullName;
            if (!parameters.ContainsKey(id))
                parameters.Add(id, Expression.Parameter(type, name));
            return parameters[id];
        }

        /// <summary>
        /// Parses the constat expression from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseConstatExpressionFromXml(XElement xml)
        {
            Type type = ParseTypeFromXml(xml.Element("Type"), resolver);
            return Expression.Constant(ParseConstantFromElement(xml, "Value", type), type);
        }

        /// <summary>
        /// Parses the type or null from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static Type ParseTypeOrNullFromXml(XElement xml)
        {
            if (IsEmptyXml(xml)) return null;

            ExpressionSerializationTypeResolver resolver = new ExpressionSerializationTypeResolver();
            return ParseTypeFromXml(xml, resolver);
        }

        /// <summary>
        /// Parses the type from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static Type ParseTypeFromXml(XElement xml, ExpressionSerializationTypeResolver resolver)
        {
            Debug.Assert(xml.Elements().Count() == 1);
            return ParseTypeFromXmlCore(xml.Elements().First(), resolver);
        }

        /// <summary>
        /// Parses the type from XML core.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Expected 'Type' or 'AnonymousType'</exception>
        public static Type ParseTypeFromXmlCore(XElement xml, ExpressionSerializationTypeResolver resolver)
        {
            switch (xml.Name.ToString())
            {
                case "Type":
                    return ParseNormalTypeFromXmlCore(xml, resolver);
                case "AnonymousType":
                    return ParseAnonymousTypeFromXmlCore(xml, resolver);
                default:
                    throw new ArgumentException("Expected 'Type' or 'AnonymousType'");
            }

        }

        /// <summary>
        /// Parses the normal type from XML core.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        static private Type ParseNormalTypeFromXmlCore(XElement xml, ExpressionSerializationTypeResolver resolver)
        {
            if (!xml.HasElements)
                return resolver.GetType(xml.Attribute("Name").Value);

            var genericArgumentTypes = from genArgXml in xml.Elements()
                                       select ParseTypeFromXmlCore(genArgXml, resolver);
            return resolver.GetType(xml.Attribute("Name").Value, genericArgumentTypes);
        }

        /// <summary>
        /// Parses the anonymous type from XML core.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        static private Type ParseAnonymousTypeFromXmlCore(XElement xElement, ExpressionSerializationTypeResolver resolver)
        {
            string name = xElement.Attribute("Name").Value;
            var properties = from propXml in xElement.Elements("Property")
                             select new ExpressionSerializationTypeResolver.NameTypePair
                             {
                                 Name = propXml.Attribute("Name").Value,
                                 Type = ParseTypeFromXml(propXml, resolver)
                             };
            var ctr_params = from propXml in xElement.Elements("Constructor").Elements("Parameter")
                             select new ExpressionSerializationTypeResolver.NameTypePair
                             {
                                 Name = propXml.Attribute("Name").Value,
                                 Type = ParseTypeFromXml(propXml, resolver)
                             };

            return resolver.GetOrCreateAnonymousTypeFor(name, properties.ToArray(), ctr_params.ToArray());
        }

        /// <summary>
        /// Parses the binary expresssion from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Expression ParseBinaryExpresssionFromXml(XElement xml)
        {
            var expressionType = (ExpressionType)ParseConstantFromAttribute<ExpressionType>(xml, "NodeType"); ;
            var left = ParseExpressionFromXml(xml.Element("Left"));
            var right = ParseExpressionFromXml(xml.Element("Right"));
            var isLifted = (bool)ParseConstantFromAttribute<bool>(xml, "IsLifted");
            var isLiftedToNull = (bool)ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
            var type = ParseTypeFromXml(xml.Element("Type"), resolver);
            var method = ParseMethodInfoFromXml(xml.Element("Method"));
            LambdaExpression conversion = ParseExpressionFromXml(xml.Element("Conversion")) as LambdaExpression;
            if (expressionType == ExpressionType.Coalesce)
                return Expression.Coalesce(left, right, conversion);
            return Expression.MakeBinary(expressionType, left, right, isLiftedToNull, method);
        }

        /// <summary>
        /// Parses the method info from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private MethodInfo ParseMethodInfoFromXml(XElement xml)
        {
            if (IsEmptyXml(xml)) return null;

            string name = (string)ParseConstantFromAttribute<string>(xml, "MethodName");
            Type declaringType = ParseTypeFromXml(xml.Element("DeclaringType"), resolver);
            var ps = from paramXml in xml.Element("Parameters").Elements()
                     select ParseTypeFromXml(paramXml, resolver);
            var genArgs = from argXml in xml.Element("GenericArgTypes").Elements()
                          select ParseTypeFromXml(argXml, resolver);
            return resolver.GetMethod(declaringType, name, ps.ToArray(), genArgs.ToArray());
        }

        /// <summary>
        /// Parses the constructor info from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private ConstructorInfo ParseConstructorInfoFromXml(XElement xml)
        {
            if (IsEmptyXml(xml)) return null;

            Type declaringType = ParseTypeFromXml(xml.Element("DeclaringType"), resolver);
            var ps = from paramXml in xml.Element("Parameters").Elements()
                     select ParseParameterFromXml(paramXml);
            ConstructorInfo ci = declaringType.GetConstructor(ps.ToArray());
            return ci;
        }

        /// <summary>
        /// Parses the parameter from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private Type ParseParameterFromXml(XElement xml)
        {
            string name = (string)ParseConstantFromAttribute<string>(xml, "Name");
            Type type = ParseTypeFromXml(xml.Element("Type"), resolver);
            return type;

        }

        /// <summary>
        /// Parses the constant from attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="attrName">Name of the attr.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">We should never be encoding Types in attributes now.</exception>
        private object ParseConstantFromAttribute<T>(XElement xml, string attrName)
        {
            string objectStringValue = xml.Attribute(attrName) == null ? "" : xml.Attribute(attrName).Value;

            //TODO: Not supported by Nhibernate3
            if (attrName == "NodeType" && objectStringValue == "ConvertChecked")
                objectStringValue = "Convert";

            if (typeof(Type).IsAssignableFrom(typeof(T)))
                throw new Exception("We should never be encoding Types in attributes now.");
            if (typeof(Enum).IsAssignableFrom(typeof(T)))
                return Enum.Parse(typeof(T), objectStringValue);
            return Convert.ChangeType(objectStringValue, typeof(T));
        }

        /// <summary>
        /// Parses the constant from attribute.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="attrName">Name of the attr.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">We should never be encoding Types in attributes now.</exception>
        private object ParseConstantFromAttribute(XElement xml, string attrName, Type type)
        {
            string objectStringValue = xml.Attribute(attrName).Value;
            if (typeof(Type).IsAssignableFrom(type))
                throw new Exception("We should never be encoding Types in attributes now.");
            if (typeof(Enum).IsAssignableFrom(type))
                return Enum.Parse(type, objectStringValue);
            return Convert.ChangeType(objectStringValue, type);
        }

        /// <summary>
        /// Parses the constant from element.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="elemName">Name of the elem.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private object ParseConstantFromElement(XElement xml, string elemName, Type type)
        {
            string objectStringValue = xml.Element(elemName).Value;
            if (typeof(Type).IsAssignableFrom(type))
                return ParseTypeFromXml(xml.Element("Value"), resolver);
            if (typeof(Enum).IsAssignableFrom(type))
                return Enum.Parse(type, objectStringValue);
            return Convert.ChangeType(objectStringValue, type);
        }
    }
}