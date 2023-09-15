using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SellManagement.DesktopClient.Services.Login;

namespace SellManagement.DesktopClient.Services
{
    public class ServiceProvider
    {
        const string STORED_TOKEN_LOCATION = "stored";

        private static ServiceProvider _instance;
        //private string _serviceEndpoint = "https://dominic-sell-management-api.herokuapp.com/";
        private string _serviceEndpoint;
        private string _accessToken;

        private ServiceProvider() {
            _serviceEndpoint = ConfigurationManager.AppSettings["ServiceEndpoint"];
            _accessToken = GetStoredToken();
        }

        public static ServiceProvider GetInstance()
        {
            if (_instance == null)
                _instance = new ServiceProvider();

            return _instance;
        }

        private string GetStoredToken()
        {
            try
            {
                return File.ReadAllText(STORED_TOKEN_LOCATION);
            }
            catch
            {
                return "";
            }
        }

        private void SetStoredToken(string token)
        {
            try
            {
                File.WriteAllText(STORED_TOKEN_LOCATION, token);
            }
            catch { }
        }

        public void Logout()
        {
            SetStoredToken("");
            _accessToken = null;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(_serviceEndpoint + "Login/authenticate");

                if (request != null)
                {
                    string jsoncontent = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = httpContent;
                }

                try
                {
                    var response = await client.SendAsync(httpRequestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                    result.StatusCode = (int)response.StatusCode;

                    if (result.StatusCode == 200)
                    {
                        _accessToken = result.Token;
                        SetStoredToken(result.Token);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    var result = JsonConvert.DeserializeObject<AuthenticateResponse>("{\"StatusCode\":\"500\",\"Message\":\"Không thể kết nối đến server\"}");
                    return result;
                }
            }
        }

        public async Task<TResponse> CallWebApi<TRequest, TResponse>(string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse
        {
            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = httpMethod;
                httpRequestMessage.RequestUri = new Uri(_serviceEndpoint + apiUrl);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                if (request != null)
                {
                    string jsoncontent = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = httpContent;
                }

                try
                {
                    var response = await client.SendAsync(httpRequestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                    result.StatusCode = (int)response.StatusCode;

                    return result;
                }
                catch
                {
                    var result = JsonConvert.DeserializeObject<TResponse>("{\"StatusCode\":\"500\",\"Message\":\"Không thể kết nối đến server\"}");
                    return result;
                }
            }
        }
    }
}
