using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Veterinaria.Core.CustomEntities
{
    public class ResponseData
    {
        public PagedList<object> Pagination { get; set; } = null!;
        public Message[] Messages { get; set; } = Array.Empty<Message>();

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}
