using GG.ErrorHandling;
using GG.ErrorHandling.Interfaces;
using GG.ResultBundle;
using Newtonsoft.Json;
using PrayerCentral.Common;
using PrayerCentral.Model.Http.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrayerCentral.Model.Http
{
    public partial class HttpContext : IAccountContext
    {
        private const int MaxRetry = 3;
        private static readonly HttpClient s_HttpClient;

        private readonly ErrorFactory<HttpContext> _ErrorFactory;
        private readonly string _ControllerName;

        internal HttpContext(string controllerName)
        {
            _ControllerName = controllerName;

            _ErrorFactory = new ErrorFactory<HttpContext>();
        }

        static HttpContext()
        {
            s_HttpClient = new HttpClient { BaseAddress = new Uri("http://10.0.1.30/GG.PrayerCentral/api/"), Timeout = TimeSpan.FromSeconds(10) };
        }

        private async Task<ResultBundle<T>> ExecuteRequest<T>(HttpRequestMessage requestMessage)
        {
            T value;
            IErrorHolder error;
            string responseValue = null;
            ResultBundle<T> resultBundle = new ResultBundle<T>(default(T));

            for (int i = 0; i < MaxRetry; i++)
            {
                try
                {
                    using (HttpResponseMessage response = await s_HttpClient.SendAsync(requestMessage))
                    {
                        responseValue = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            responseValue = await response.Content.ReadAsStringAsync();

                            value = JsonConvert.DeserializeObject<T>(responseValue);

                            resultBundle = new ResultBundle<T>(value);
                        }
                        else
                        {
                            error = _ErrorFactory.CreateError(ErrorCodes.RequestFailure, responseValue);

                            resultBundle = new ResultBundle<T>(default(T), error);
                        }
                    }
                }
                catch (Exception e)
                {
                    error = _ErrorFactory.CreateError(ErrorCodes.Exception, e);

                    resultBundle = new ResultBundle<T>(default(T), error);
                }
            }

            return resultBundle;
        }
    }
}
