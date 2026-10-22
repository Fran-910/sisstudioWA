using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Servicios.ServicioHttp
{
    public interface IHttpService
    {
        Task<HttpResp<T>> GetAsync<T>(string url);
        Task<HttpResp<TResp>> PostAsync<T, TResp>(string url, T data);
        Task<HttpResp<string>> PostFormDataAsync(string url, MultipartFormDataContent data);
        Task<HttpResp<string>> UpdateFormDataAsync(string url, MultipartFormDataContent data);
        Task<HttpResp<TResp>> PutAsync<T, TResp>(string url, T data);
        Task<HttpResp<T>> DeleteAsync<T>(string url);
        Task<bool> DeleteImg(string url, string imgdtoJson);
    }
}
