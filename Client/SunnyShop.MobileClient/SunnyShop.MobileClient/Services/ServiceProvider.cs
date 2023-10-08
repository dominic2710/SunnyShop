
using SunnyShop.MobileClient.Helpers;
using SunnyShop.MobileClient.Services.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SunnyShop.MobileClient.Services
{
    public class ServiceProvider
    {
        private static ServiceProvider _instance;
        //private string _serverRootUrl = "https://10.0.2.2:7093";
        public string _accessToken = "";
        private DevHttpsConnectionHelper _devSslHelper;
        private ServiceProvider() {
            _devSslHelper = new DevHttpsConnectionHelper(sslPort: 5001);

            //TODO: Remove fixed value and add LoginPage
            _accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE2OTY2OTM3MTYsImV4cCI6MTY5NzI5ODUxNiwiaWF0IjoxNjk2NjkzNzE2fQ.5ikrUsr4pNdnRJuIehFjCBkM5jesyXGBlMihWahNE4o";
        }

        public static ServiceProvider GetInstance()
        {
            if (_instance == null)
                _instance = new ServiceProvider();

            return _instance;
        }

        //public ServiceProvider()
        //{
        //    _devSslHelper = new DevHttpsConnectionHelper(sslPort: 7012);
        //}

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + "/Authenticate/Authenticate");

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == 200)
                {
                    _accessToken = result.Token;

                    _devSslHelper.HttpClient.DefaultRequestHeaders.Authorization =
                                        new AuthenticationHeaderValue("Bearer", _accessToken);

                }
                return result;
            }
            catch (Exception ex)
            {
                var result = new LoginResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
                return result;
            }
        }

        public async Task<TResponse> CallWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + apiUrl);
            httpRequestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResponse>();
                result.StatusCode = 500;
                result.StatusMessage = ex.Message;
                return result;
            }
        }

        public async Task<byte[]> GetByteArrayAsync(string apiUrl)
        {
            var photoUri = _devSslHelper.DevServerRootUrl + apiUrl;
            try
            {
                var imageBytes = await _devSslHelper.HttpClient.GetByteArrayAsync(photoUri);
                return imageBytes;
            }
            catch
            {
                byte[] result = null;
                return result;
            }
        }

    }
}
