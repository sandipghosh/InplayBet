
namespace InplayBet.Web.Utilities
{
    using System;
    using System.Web;

    public static class SessionVeriables
    {
        private static string _UserKey = "USERKEY";
        private static string _UserId = "USERID";
        private static string _UserName = "USERNAME";
        private static string _IsAdmin = "ISADMIN";

        /// <summary>
        /// Gets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public static string UserKey
        {
            get
            {
                return _UserKey;
            }
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public static string UserId
        {
            get
            {
                return _UserId;
            }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public static string UserName
        {
            get
            {
                return _UserName;
            }
        }

        public static string IsAdmin
        {
            get
            {
                return _IsAdmin;
            }
        }

        /// <summary>
        /// Gets the session data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetSessionData<T>(string key)
        {

            return (HttpContext.Current.Session[key] == null)
                ? default(T) : (T)Convert.ChangeType(HttpContext.Current.Session[key], typeof(T));
        }

        /// <summary>
        /// Sets the session data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public static void SetSessionData<T>(string key, T data)
        {
            HttpContext.Current.Session[key] = data;
        }
    }
}