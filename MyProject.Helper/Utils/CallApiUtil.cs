using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Helper.Constants.Globals;
using MyProject.Helper.Utils.Interfaces;
using Newtonsoft.Json;

namespace MyProject.Helper.Utils
{
    public class CallApiUtil : ICallApiUtils
    {
        public HttpRequestMessage CallApi(string url, string apiKey, string requestBody)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "accept", "application/json" },
                    { "x-api-key", apiKey},
                },
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            return request;
        }
    }
}
