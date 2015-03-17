
namespace InplayBet.Web.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Text.RegularExpressions;

    public static class CommonUtility
    {
        private static Random rand = new Random();
        private static int seed = Environment.TickCount;
        public const string DEFAULT_ALLOWED_CHARACTER = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const int DEFAULT_LENGTH = 8;

        /// <summary>
        /// The random wrapper
        /// </summary>
        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref seed))
        );

        /// <summary>
        /// Gets the thread random.
        /// </summary>
        /// <returns></returns>
        private static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }

        /// <summary>
        /// Gets the random string.
        /// </summary>
        /// <param name="rnd">The RND.</param>
        /// <param name="allowedChars">The allowed chars.</param>
        /// <param name="minLength">Length of the min.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetRandomString(Random rnd,
            string allowedChars = DEFAULT_ALLOWED_CHARACTER,
            int minLength = DEFAULT_LENGTH,
            int maxLength = DEFAULT_LENGTH, int count = 1)
        {
            char[] chars = new char[maxLength];
            int setLength = allowedChars.Length;

            while (count-- > 0)
            {
                int length = rnd.Next(minLength, maxLength + 1);
                for (int i = 0; i < length; ++i)
                {
                    chars[i] = allowedChars[rnd.Next(setLength)];
                }
                yield return new string(chars, 0, length);
            }
        }

        /// <summary>
        /// Genarates the random string.
        /// </summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        public static string GenarateRandomString
            (int minLength = DEFAULT_LENGTH,
            int maxLength = DEFAULT_LENGTH)
        {
            //int seed = (int)DateTime.Now.Ticks;
            //Random rnd = new Random(seed);
            //return GetRandomString(rnd, DEFAULT_ALLOWED_CHARACTER, minLength, maxLength).First();

            return GetRandomString(GetThreadRandom(), DEFAULT_ALLOWED_CHARACTER, minLength, maxLength).First();
        }

        /// <summary>
        /// Determines whether [contains search ex] [the specified search context].
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool ContainsSearchEx(this string searchContext, string searchWith)
        {
            return searchContext.IndexOf(searchWith, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
        /// <summary>
        /// Nots the contains search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool NotContainsSearchEx(this string searchContext, string searchWith)
        {
            return !(searchContext.IndexOf(searchWith, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
        /// <summary>
        /// Equalses the search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool EqualsSearchEx(this string searchContext, string searchWith)
        {
            return searchContext.Equals(searchWith, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Nots the equals search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool NotEqualsSearchEx(this string searchContext, string searchWith)
        {
            return !searchContext.Equals(searchWith, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Startses the with search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool StartsWithSearchEx(this string searchContext, string searchWith)
        {
            return searchContext.StartsWith(searchWith, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Nots the starts with search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool NotStartsWithSearchEx(this string searchContext, string searchWith)
        {
            return !searchContext.StartsWith(searchWith, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Endses the with search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool EndsWithSearchEx(this string searchContext, string searchWith)
        {
            return searchContext.EndsWith(searchWith, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Nots the ends with search ex.
        /// </summary>
        /// <param name="searchContext">The search context.</param>
        /// <param name="searchWith">The search with.</param>
        /// <returns></returns>
        public static bool NotEndsWithSearchEx(this string searchContext, string searchWith)
        {
            return !searchContext.EndsWith(searchWith, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Gets the type of the compatible data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Type GetCompatibleDataType(this string value)
        {
            Decimal outTypeDecimal;
            int outTypeInteger;
            DateTime outTypeDate;
            bool outTypeBoolean;

            if (Decimal.TryParse(value, out outTypeDecimal))
                return typeof(Decimal);
            else if (int.TryParse(value, out outTypeInteger))
                return typeof(int);
            else if (DateTime.TryParse(value, out outTypeDate))
                return typeof(DateTime);
            else if (bool.TryParse(value, out outTypeBoolean))
                return typeof(bool);
            else
                return typeof(string);
        }


        /// <summary>
        /// Gets the lamda expression from filter.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="strFilter">The string filter.</param>
        /// <returns></returns>
        public static Expression<Func<TModel, bool>> GetLamdaExpressionFromFilter<TModel>(string strFilter)
        {
            try
            {
                strFilter = strFilter.IsBase64() ? strFilter.ToBase64Decode() : strFilter;
                Expression<Func<TModel, bool>> filterExp = InplayBet.Web.Utilities.Expression
                    .ExpressionBuilder.BuildLamdaExpression<TModel, bool>(strFilter);
                return filterExp;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(strFilter);
            }
            return null;
        }

        /// <summary>
        /// Excludes the state of from model.
        /// </summary>
        /// <param name="epxpression">The epxpression.</param>
        /// <param name="state">The state.</param>
        public static void ExcludeFromModelState<TModel, TProperty>
            (Expression<Func<TModel, TProperty>> epxpression, ModelStateDictionary state)
        {
            try
            {
                NewExpression newExp = epxpression.Body as NewExpression;
                var excludedFields = newExp.Arguments.Select(x => (x as MemberExpression).Member.Name).ToList();

                state.Keys.Where(x => excludedFields.Contains(x.Split('.')[1])).ToList()
                    .ForEach(x => { state[x].Errors.Clear(); });
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(epxpression, state);
            }
        }

        /// <summary>
        /// Sets the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="context">The context.</param>
        public static void SetPropertiesFromContext<T>(this T source, HttpContext context)
        {
            try
            {
                var properties = typeof(T)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public);
                NameValueCollection values = null;

                if (context.Request.HttpMethod.ToUpper() == HttpVerbs.Post.ToString().ToUpper())
                    values = context.Request.Form;
                else if (context.Request.HttpMethod.ToUpper() == HttpVerbs.Get.ToString().ToUpper())
                    values = context.Request.QueryString;

                foreach (var prop in properties)
                {
                    if (values.AllKeys.Contains(prop.Name))
                    {
                        Type propType = prop.PropertyType;
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            if (!String.IsNullOrEmpty(values[prop.Name]))
                            {
                                var value = Convert.ChangeType(values[prop.Name], propType.GetGenericArguments()[0]);
                                prop.SetValue(source, value, null);
                            }
                        }
                        else
                        {
                            if (propType.Namespace.StartsWith("System"))
                            {
                                var value = Convert.ChangeType(values[prop.Name], propType);
                                prop.SetValue(source, value, null);
                            }
                            else
                            {
                                string propValue = values[prop.Name].IsBase64() ?
                                    values[prop.Name].ToBase64Decode() : values[prop.Name].ToString();

                                var value = JsonConvert.DeserializeObject(propValue, propType);
                                prop.SetValue(source, value, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(source, context);
            }
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static NameValueCollection GetContext(HttpContext context)
        {
            NameValueCollection values = null;

            if (context.Request.HttpMethod.ToUpper() == HttpVerbs.Post.ToString().ToUpper())
                values = context.Request.Form;

            else if (context.Request.HttpMethod.ToUpper() == HttpVerbs.Get.ToString().ToUpper())
                values = context.Request.QueryString;

            return values;
        }

        /// <summary>
        /// Logs to file.
        /// </summary>
        /// <param name="logContent">Content of the log.</param>
        public static void LogToFileWithStack(string logContent)
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string baseDir = HttpContext.Current == null ?
                string.Format("{0}{1}\\", AppDomain.CurrentDomain.BaseDirectory, CommonUtility.GetConfigData<string>("ErrorLogFolder")) :
                HttpContext.Current.Server.MapPath(string.Format("~/{0}/", CommonUtility.GetConfigData<string>("ErrorLogFolder")));

            string logFilePath = string.Format("{0}LogFile-{1}{2}{3}-{4}{5}{6}.txt", baseDir,
                DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year,
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            StackFrame frame = new StackFrame(1, true);
            MethodBase lastCalling = frame.GetMethod();
            string lastCallingFunction = lastCalling.Name;
            string callingModule = lastCalling.Module.Name;
            string fileName = frame.GetFileName();
            string lineNumber = frame.GetFileLineNumber().ToString();

            FileLogger log = new FileLogger(logFilePath, true, FileLogger.LogType.TXT, FileLogger.LogLevel.All);
            ThreadPool.QueueUserWorkItem((state) =>
                log.LogRaw(string.Format("{0} :{1}-{2}; {3}.{4} ==> {5}", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    fileName, lineNumber, callingModule, lastCallingFunction, logContent)));
        }

        /// <summary>
        /// Exceptions the value tracker.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="parameters">The parameters.</param>
        public static void ExceptionValueTracker(this Exception ex, params object[] parameters)
        {
            StackFrame stackFrame = new StackTrace().GetFrame(1);
            var methodInfo = stackFrame.GetMethod();
            var paramInfos = methodInfo.GetParameters();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("[Function: {0}.{1}]", methodInfo.DeclaringType.FullName, methodInfo.Name));
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var currentParameterInfo = paramInfos[i];

                string paramValue = string.Empty;
                if (parameters.Length - 1 >= i)
                {
                    var currentParameter = parameters[i];
                    if (parameters[i] != null)
                    {
                        paramValue = (currentParameter.GetType().Namespace.StartsWith("System")) ?
                            currentParameter.ToString() : JsonConvert.SerializeObject(currentParameter,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                });
                    }
                }

                sb.AppendLine(string.Format("   {0} : {1}", currentParameterInfo.Name, paramValue));
            }
            sb.AppendLine("[Function End]");

            ex.Data.Clear();
            ex.Data.Add("FunctionInfo", sb.ToString());

            if (CommonUtility.GetConfigData<string>("EnableErrorLog").ToLower() == "true")
            {
                LogToFileWithStack(string.Format("{0}\r\n{1}", ex.Message, sb.ToString()));
            }
        }

        /// <summary>
        /// Exceptions the value tracker.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="parameters">The parameters.</param>
        public static void ExceptionValueTracker(this Exception ex, IDictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Application Error Starts]");

            foreach (KeyValuePair<string, object> item in parameters)
            {
                string paramValue = (item.Value.GetType().Namespace.StartsWith("System")) ?
                    item.Value.ToString() : JsonConvert.SerializeObject(item.Value, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                sb.AppendLine(string.Format("   {0} : {1}", item.Key, paramValue));
            }
            sb.AppendLine("[Application Error Ends]");

            ex.Data.Clear();
            ex.Data.Add("FunctionInfo", sb.ToString());

            if (CommonUtility.GetConfigData<string>("EnableErrorLog").ToLower() == "true")
            {
                LogToFileWithStack(string.Format("{0}\r\n{1}", ex.Message, sb.ToString()));
            }
        }

        /// <summary>
        /// Toes the base64 encode.
        /// </summary>
        /// <param name="toEncode">To encode.</param>
        /// <returns></returns>
        public static string ToBase64Encode(this string toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(toEncode);
            }
            return string.Empty;
        }

        /// <summary>
        /// Toes the base64 decode.
        /// </summary>
        /// <param name="encodedData">The encoded data.</param>
        /// <returns></returns>
        public static string ToBase64Decode(this string encodedData)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                string returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
                return HttpContext.Current.Server.UrlDecode(returnValue);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(encodedData);
            }
            return string.Empty;
        }

        /// <summary>
        /// Determines whether the specified base64 string is base64.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns></returns>
        public static bool IsBase64(this string base64String)
        {
            if (base64String.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                exception.ExceptionValueTracker(base64String);
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is ajax request] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return ((request.Headers.AllKeys.Contains("x-requested-with") &&
                request.Headers["x-requested-with"] == "XMLHttpRequest") ||
                (request.Headers.AllKeys.Contains("x-my-custom-header") &&
                request.Headers["x-my-custom-header"] == "AjaxRequest"));
        }

        /// <summary>
        /// Renders the view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="additionalData">The additional data.</param>
        /// <returns></returns>
        public static string RenderViewToString(string viewName, object viewData,
            ControllerBase controller, IDictionary<string, object> additionalData)
        {
            try
            {
                HttpContextBase contextBase = new HttpContextWrapper(HttpContext.Current);
                TempDataDictionary tempData = new TempDataDictionary();

                foreach (var item in additionalData)
                {
                    tempData[item.Key] = item.Value;
                }

                var routeData = new RouteData();
                routeData.Values.Add("controller", controller.GetType().Name.Replace("Controller", ""));
                var controllerContext = new ControllerContext(contextBase, routeData, controller);

                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(controllerContext, viewName, "", false);

                var writer = new StringWriter();
                var viewContext = new ViewContext(controllerContext, razorViewResult.View,
                       new ViewDataDictionary(viewData), tempData, writer);
                razorViewResult.View.Render(viewContext, writer);

                return writer.ToString();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(viewName, viewData, controller, additionalData);
            }
            return string.Empty;
        }

        /// <summary>
        /// Saves the image from data URL.
        /// </summary>
        /// <param name="urlData">The URL data.</param>
        /// <param name="userKey">The user key.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static string SaveImageFromDataUrl(string urlData, int userKey, string userId)
        {
            try
            {
                Func<ImageFormat, ImageCodecInfo> GetImageEncoder = (IF) =>
                    ImageCodecInfo.GetImageEncoders().FirstOrDefaultCustom(x => x.FormatID.Equals(IF.Guid));

                var base64Data = Regex.Match(urlData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                if (!string.IsNullOrEmpty(base64Data))
                {
                    byte[] byteArray = Convert.FromBase64String(base64Data);

                    string imageRelativePath = string.Format("~/Images/Users/{0}/{1}.jpg", userKey, userId);
                    string destinationImagePath = HttpContext.Current.Server.MapPath(imageRelativePath);

                    if (!Directory.Exists(Path.GetDirectoryName(destinationImagePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationImagePath));

                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        using (Bitmap bmp = new Bitmap(ms))
                        {
                            ImageCodecInfo jgpEncoder = GetImageEncoder(ImageFormat.Jpeg);
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            try
                            {
                                bmp.Save(destinationImagePath, jgpEncoder, myEncoderParameters);
                            }
                            catch (Exception)
                            {
                                return urlData;
                            }
                        }

                        //using (Image returnImage = Image.FromStream(ms))
                        //{
                        //    returnImage.Save(imagePath, ImageFormat.Jpeg);
                        //    returnImage.Dispose();
                        //}
                        //ms.Close();
                        //ms.Dispose();
                    }

                    return imageRelativePath;
                }
                return urlData;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(urlData, userKey, userId);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the configuration data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetConfigData<T>(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }

        /// <summary>
        /// Appsettingses to json.
        /// </summary>
        /// <returns></returns>
        public static string AppsettingsToJson()
        {
            return JsonConvert.SerializeObject(ConfigurationManager.AppSettings
                .AllKeys.Where(x => !x.Contains(":"))
                .ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]));
        }

        public static bool IsFollowing(int userKey)
        {
            try
            {
                int loggedInUser = SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey);
                if (loggedInUser > 0 && loggedInUser != userKey)
                {
                    SharedFunctionality shared = new SharedFunctionality();
                    return shared.IsFollowing(loggedInUser, userKey);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return true;
        }
    }
}