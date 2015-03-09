
namespace InplayBet.Web.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Web.Mvc;

    public class JsonActionResult : JsonResult
    {
        public JsonActionResult(object data)
        {
            base.Data = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            response.Write(serializedObject);
        }
    }
}