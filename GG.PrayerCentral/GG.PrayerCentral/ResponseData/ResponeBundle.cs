using GG.PrayerCentral.EnumsAndConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.PrayerCentral.ResponseData
{
    public class ResponeBundle<T>
    {
        public string Error { get; }
        public ResponseStatus Response { get; } = ResponseStatus.Success;
        public T Result { get; }

        public ResponeBundle(T result)
        {
            Result = result;
        }

        public ResponeBundle(T result, ResponseStatus response, string error)
        {
            Result = result;
            Response = response;
            Error = error;
        }
    }
}
