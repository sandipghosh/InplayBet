
namespace InplayBet.Web.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web;

    public static class CommonUtility
    {
        /// <summary>
        /// Logs to file.
        /// </summary>
        /// <param name="logContent">Content of the log.</param>
        public static void LogToFileWithStack(string logContent)
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string baseDir = HttpContext.Current == null ?
                string.Format("{0}{1}\\", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ErrorLogFolder"]) :
                HttpContext.Current.Server.MapPath(string.Format("~/{0}/", ConfigurationManager.AppSettings["ErrorLogFolder"]));

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
            log.LogRaw(string.Format("{0} :{1}-{2}; {3}.{4} ==> {5}", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                fileName, lineNumber, callingModule, lastCallingFunction, logContent));
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

            if (ConfigurationManager.AppSettings["EnableErrorLog"].ToString().ToLower() == "true")
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

            if (ConfigurationManager.AppSettings["EnableErrorLog"].ToString().ToLower() == "true")
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
    }
}