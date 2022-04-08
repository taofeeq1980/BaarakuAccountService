using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Shared.Helpers
{
    public abstract class PaystackAdapter
    {
        private static readonly HttpClient _client = new();
        private readonly IConfiguration _config;
        private readonly ILogger<PaystackAdapter> _logger;
        private readonly string _token;

        public PaystackAdapter(IConfiguration config, ILogger<PaystackAdapter> logger)
        {
            _config = config;
            _token = _config["PayStackSettings:Secret_Key"];
            _logger = logger;
        }

        protected async Task<T> PostResponse<T>(string url, object model) where T : class
        {
            try
            {
                _logger.LogInformation(JsonConvert.SerializeObject(model));
                var result = new HttpResponseMessage();
                HttpClient _client = new();
                _client.SetBearerToken(_token);
                result = await _client.PostAsJsonAsync(url, model);

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(await result.Content.ReadAsStringAsync());
                    return await result.Content.ReadAsAsync<T>();
                }
                else
                {
                    var strRes = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(strRes);
                    try
                    {
                        var rawResponse = await result.Content.ReadAsAsync<T>();
                        return rawResponse;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
        protected async Task<T> GetResponse<T>(string url) where T : class
        {
            try
            {
                _logger.LogInformation(url);
                var result = new HttpResponseMessage();
                _client.SetBearerToken(_token);
                result = await _client.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    var strRes = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(strRes);
                    return await result.Content.ReadAsAsync<T>();
                }
                else
                {
                    var strRes = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(strRes);
                    try
                    {
                        var rawResponse = await result.Content.ReadAsAsync<T>();
                        return rawResponse;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
        protected async Task<T> PutResponse<T>(string url, object model) where T : class
        {
            try
            {
                var result = new HttpResponseMessage();
                _client.SetBearerToken(_token);
                result = await _client.PutAsJsonAsync(url, model);

                if (result.IsSuccessStatusCode)
                {
                    var strRes = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(strRes);
                    return await result.Content.ReadAsAsync<T>();
                }
                else
                {
                    var strRes = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(strRes);
                    try
                    {
                        var rawResponse = await result.Content.ReadAsAsync<T>();
                        return rawResponse;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
