using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Cqrs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Monkey.WebApi.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private class RequestExceptionActionResult : ActionResult
        {
            private readonly ErrorCodeReason _statusCode;
            private readonly string _errorMessage;
            private readonly int _errorCode;

            public RequestExceptionActionResult(
                ErrorCodeReason statusCode,
                string errorMessage,
                int errorCode)
            {
                _statusCode = statusCode;
                _errorMessage = errorMessage;
                _errorCode = errorCode;
            }

            public override void ExecuteResult(ActionContext context)
            {
                context.HttpContext.Response.StatusCode = (int)_statusCode;
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var error = JsonConvert.SerializeObject(new ErrorResponse { Code = _errorCode, Message = _errorMessage }, serializerSettings);
                StreamWriter sw = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8, 1024, true);
                sw.Write(error);
                sw.Flush();
            }
        }
        private class XmlRequestExceptionActionResult : ActionResult
        {
            private readonly ErrorCodeReason _statusCode;
            private readonly XDocument _errorMessage;
            

            public XmlRequestExceptionActionResult(
                ErrorCodeReason statusCode,
                XDocument message)
            {
                _statusCode = statusCode;
                _errorMessage = message;
                
            }

            public override void ExecuteResult(ActionContext context)
            {
                context.HttpContext.Response.StatusCode = (int)_statusCode;
                
                var error = JsonConvert.SerializeXNode(_errorMessage);
                StreamWriter sw = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8, 1024, true);
                sw.Write(error);
                sw.Flush();
            }
        }
        private class ErrorResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case XmlRequestException ex:
                    context.Result = new XmlRequestExceptionActionResult(ex.StatusCode, ex.XmlMessage);
                    context.ExceptionHandled = true;
                    return;
                case RequestException re:
                    context.Result = new RequestExceptionActionResult(re.StatusCode, re.Message, re.ErrorCode);
                    context.ExceptionHandled = true;
                    return;
            }
        }
    }
}
