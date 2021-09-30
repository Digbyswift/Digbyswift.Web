using System;
using System.IO;
using System.Web.Mvc;
using Digbyswift.Web.Extensions;
using Digbyswift.Web.Mvc.Extensions;
using Newtonsoft.Json;

namespace Digbyswift.Web.Mvc.ActionResults
{
    public class CustomJsonResult : JsonResult
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public CustomJsonResult()
        {
        }

        public CustomJsonResult(object data, JsonSerializerSettings serializerSettings = null)
        {
            Data = data;
            _serializerSettings = serializerSettings;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && context.HttpContext.Request.IsGetMethod())
                throw new InvalidOperationException("JSON GET is not allowed");

            var response = context.HttpContext.Response;
            response.ContentType = String.IsNullOrWhiteSpace(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            using (var sw = new StringWriter())
            {
                sw.Write(_serializerSettings != null
                    ? JsonConvert.SerializeObject(Data, _serializerSettings)
                    : JsonConvert.SerializeObject(Data));

                response.Write(sw.ToString());
            }
        }
    }
}