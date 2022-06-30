using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Core;
using IsolatedAzureFunctionAppDemo.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IsolatedAzureFunctionAppDemo
{
    public class DataPostBody
    {
        public string? name { get;set; }    
    }

    public class Test
    {
        private readonly ILogger _logger;
        private readonly IData _data;

        public Test(ILoggerFactory loggerFactory, IData data)
        {
            _logger = loggerFactory.CreateLogger<Test>();
            _data = data;
        }

        [Function("GetData")]
        public HttpResponseData RunGet([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "data")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            response.WriteString(_data.AsJson());

            return response;
        }
        
        [Function("PostData")]
        public async Task<HttpResponseData> RunPost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "data")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            string? bodyString = await req.ReadAsStringAsync();
            var bodyJson = Newtonsoft.Json.JsonConvert.DeserializeObject<DataPostBody>(bodyString);
            
            _data.Id = _data.Id == null ? 1 : _data.Id + 1;
            _data.Name = bodyJson.name;

            var logInfo = string.Format("Updated object and incremented id to {0}", _data.Id);
            _logger.LogInformation(logInfo);

            await response.WriteStringAsync("Ok");
            return response;
        }
    }
}
