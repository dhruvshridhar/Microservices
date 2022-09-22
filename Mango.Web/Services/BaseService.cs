using System;
using System.Text;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        // Recommended client in dotnet core
        public IHttpClientFactory HttpClient { get; set; }
        public ResponseDTO ResponseModel { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.HttpClient = httpClient;
            this.ResponseModel = new ResponseDTO();
        }

        public async Task<T> SendAsync<T>(ApiRequest request)
        {
            try
            {
                var client = HttpClient.CreateClient("MangoAPI");
                var message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.Url);
                client.DefaultRequestHeaders.Clear();

                if(request.Data is not null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),Encoding.UTF8,"application/json");
                }

                HttpResponseMessage httpResponse = null;
                switch (request.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put; break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete; break;
                    default:
                        message.Method = HttpMethod.Get; break;
                }

                httpResponse = await client.SendAsync(message);
                var apiContent = await httpResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponseDto;
            }
            catch(Exception e)
            {
                var dto = new ResponseDTO
                {
                    Errors = new List<string> { e.Message },
                    Message = "Error",
                    IsSuccess = false
                };
                var resp = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(resp);
                return apiResponse;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}

