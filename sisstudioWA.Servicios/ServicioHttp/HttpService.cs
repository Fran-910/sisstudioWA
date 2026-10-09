using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sisstudioWA.Servicios.ServicioHttp
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient http;

        public HttpService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<HttpResp<T>> DeleteAsync<T>(string url)
        {
            try
            {
                var response = await http.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<T>();
                    return new HttpResp<T>(true, string.Empty, data, response);
                }
                else
                {
                    return new HttpResp<T>(false, string.Empty, default, response);
                }
            }
            catch (Exception e)
            {
                return new HttpResp<T>(false, e.Message, default, null);
            }
        }

        public async Task<HttpResp<T>> GetAsync<T>(string url)
        {
            try
            {
                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<T>();
                    return new HttpResp<T>(true, string.Empty, data, response);   
                }
                else
                {
                    return new HttpResp<T>(false, string.Empty, default, response);
                }
            }
            catch (Exception e)
            {
                return new HttpResp<T>(false, e.Message, default, null);
            }

        }

        public async Task<HttpResp<TResp>> PostAsync<T, TResp>(string url, T data)
        {
            try
            {
                var response = await http.PostAsJsonAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<TResp>();
                    return new HttpResp<TResp>(true, string.Empty, responseData, response);
                }
                else
                {
                    return new HttpResp<TResp>(false, string.Empty, default, response);
                }   
            }
            catch (Exception e)
            {
                return new HttpResp<TResp>(false, e.Message, default, null);
            }
        }

        public async Task<HttpResp<TResp>> PutAsync<T, TResp>(string url, T data)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await http.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var deserializedData = JsonSerializer.Deserialize<TResp>(responseData);

                    return new HttpResp<TResp>(true, string.Empty, deserializedData, response);
                }
                else
                {
                    return new HttpResp<TResp>(false, string.Empty, default, response);
                }
            }
            catch (Exception e)
            {
                return new HttpResp<TResp>(false, e.Message, default, null);
            }
        }
    }
}
