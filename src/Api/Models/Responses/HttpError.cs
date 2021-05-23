using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Models.Responses
{
    public class HttpError
    {
        [JsonPropertyName("error")]
        public string Error { get; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; }

        public HttpError(string error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
