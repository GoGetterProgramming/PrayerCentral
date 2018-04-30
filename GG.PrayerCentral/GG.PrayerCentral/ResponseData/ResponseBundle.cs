using GG.PrayerCentral.EnumsAndConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.PrayerCentral.ResponseData
{
    public class ResponseBundle<T>
    {
        public string Error { get; }
        public ResponseStatus Response { get; } = ResponseStatus.Success;
        public T Result { get; }

        public ResponseBundle(T result)
        {
            Result = result;
        }

        public ResponseBundle(T result, ResponseStatus response, string error)
        {
            Result = result;
            Response = response;
            Error = error;
        }
    }
}
