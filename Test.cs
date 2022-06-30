using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure.Core;
using IsolatedAzureFunctionAppDemo.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [Function("GetParams")]
        public HttpResponseData RunGetParams([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "params")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            var dict = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
            var person = JsonConvert.DeserializeObject<Person>(json);
            
            response.WriteString($"Name: [{person.name}] Age: [{person.age}]");

            return response;
        }

        [Function("DolphinResource")]
        public HttpResponseData DolphinResource(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "dolphin/{client:int}/{enterprise:int}")]
                HttpRequestData req,
                int client,
                int enterprise
            )
        {
            string result = null;
            switch (req.Method)
            {
                case "PUT":
                    result = DolphinService.create();
                    break;
                case "GET":
                    result = DolphinService.read();
                    break;
                case "POST":
                    result = DolphinService.update();
                    break;
                case "DELETE":
                    result = DolphinService.delete();
                    break;
            }

            var person = GetQueryAsObject<Person>(req.Url.Query);
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString($"Client: [{client}] Enterprise: [{enterprise.ToString()}] Method: [{result}] Person: [{person.name}] Age: [{person.age}]");
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

        private T GetQueryAsObject<T>(string query)
        {
            var dict = System.Web.HttpUtility.ParseQueryString(query);
            string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    public class Person
    {
        public Person()
        {
            name = "default";
            age = 0;
        }
        
        public string name { get; set; }
        public int age { get; set; }
    }
}
