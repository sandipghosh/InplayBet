
namespace InplayBet.Web.Utilities
{
    using System;
    using System.Web;

    public static class SessionVeriables
    {
        private static string _UserKey = "USERKEY";

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
        /// Gets the session data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetSessionData<T>(string key)
        {
            
            return (HttpContext.Current.Session[key] == null)
                ? default(T) : (T)Convert.ChangeType(HttpContext.Current.Session[key], typeof(T));
        }

        public static void SetSessionData<T>(string key, T data)
        {
            HttpContext.Current.Session[key] = data;
        }
    }
}