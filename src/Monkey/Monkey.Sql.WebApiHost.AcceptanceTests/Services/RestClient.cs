using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.Services
{
    public class RestClient
    {
        private static HttpClient Client = new HttpClient { Timeout = TimeSpan.FromMinutes(2) };
        protected Uri BaseAddress;
        //private IAccessTokenProvider _accessTokenProvider;

        public RestClient(/*IAccessTokenProvider accessTokenProvider*/)
        {
            //_accessTokenProvider = accessTokenProvider;
        }

        public async Task<HttpResponseMessage> Get(string path, object request = null, params Header[] headers)
        {
            var baseAddress = BaseAddress;
            var queryString = request != null
                ? ToQueryString(request)
                : string.Empty;

            var requestMessage = RequestMessage.Get($"{baseAddress}{path}?{queryString}".TrimEnd('?'), headers ?? new Header[0]);

            //await AddAuthorizatioBeareToken(requestMessage);
            return await Client.SendAsync(requestMessage);
        }
        public async Task<HttpResponseMessage> Post(string path, object request = null, params Header[] headers)
        {
            var baseAddress = BaseAddress;
            //var arg = request != null ? JsonConvert.SerializeObject(request) : null;
            var requestMessage = RequestMessage.Post($"{baseAddress}{path}".TrimEnd('?'), request, headers ?? new Header[0]);

            //await AddAuthorizatioBeareToken(requestMessage);
            return await Client.SendAsync(requestMessage);
        }
        //private async Task AddAuthorizatioBeareToken(HttpRequestMessage httpRequest)
        //{
        //    var usersAccessToken = await _accessTokenProvider.Get();
        //    if (usersAccessToken != null)
        //    {
        //        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{usersAccessToken}");
        //    }
        //}
        private string ToQueryString(object request, string separator = "&")
        {
            // Get all properties on the object
            var properties = request.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(request, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                    ? valueType.GetGenericArguments()[0]
                    : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
        }
    }
}