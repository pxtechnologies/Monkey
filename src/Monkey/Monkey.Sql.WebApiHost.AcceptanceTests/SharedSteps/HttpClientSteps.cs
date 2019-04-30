using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Monkey.Sql.WebApiHost.AcceptanceTests.Services;
using Monkey.WebApi;
using Newtonsoft.Json;
using NSubstitute.Core;
using NUnit.Framework;
using Swashbuckle.AspNetCore.Swagger;
using TechTalk.SpecFlow;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.SharedSteps
{
    [Binding]
    public class HttpClientSteps
    {
        private readonly ScenarioContext _sceneario;
        private readonly string _baseUrl;
        private readonly HttpClient _client;
        public HttpClientSteps(ScenarioContext sceneario, IConfiguration config)
        {
            _sceneario = sceneario;
            _baseUrl = config.GetApplicationUrls().First().Replace("*", "localhost");
            _client = new HttpClient();
        }

        [When(@"I invoke WebApi with '(.*)' request on '(.*)' without data")]
        public async Task WhenIInvokeWebApiWithRequestOn(HttpVerb verb, string url)
        {
            await WhenIInvokeWebApiWithRequestOnWithData(verb, url, null);
        }

        [When(@"I invoke WebApi with '(.*)' request on '(.*)' with data '(.*)'")]
        public async Task WhenIInvokeWebApiWithRequestOnWithData(HttpVerb verb, string url, string json)
        {
            if (verb == HttpVerb.POST)
            {
                var serviceUrl = $"{_baseUrl}{url}";
                var result = await _client.PostAsync(serviceUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                _sceneario[url] = result;
            }
            else if (verb == HttpVerb.PUT)
            {
                var serviceUrl = $"{_baseUrl}{url}";
                var result = await _client.PutAsync(serviceUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                _sceneario[url] = result;

            }
            else if (verb == HttpVerb.GET)
            {
                var serviceUrl = $"{_baseUrl}{url}";
                var result = await _client.GetAsync(serviceUrl);
                int ix = url.IndexOf('?');
                if(ix > 0)
                    _sceneario[url.Remove(ix)] = result;
                else
                    _sceneario[url] = result;
            }
            else
            {
                
                throw new NotImplementedException();
            }
        }
        [Then(@"I expect a response from url '(.*)' with data '(.*)'")]
        public async Task ThenIExpectAResponseFromUrlWithData(string url, string json)
        {
            //await Task.Delay(100000);
            HttpResponseMessage response = (HttpResponseMessage) _sceneario[url];
            ((int) response.StatusCode).Should().BeLessThan(300);
            var str = await response.Content.ReadAsStringAsync();
            str = str.RemoveWhiteSpaces();
            str.Should().Be(json.RemoveWhiteSpaces());
        }

    }
}
