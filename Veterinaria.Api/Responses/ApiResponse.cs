using Veterinaria.Core.CustomEntities;

namespace Veterinaria.Api.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Pagination? Pagination { get; set; }
        public List<string>? Messages { get; set; }
    }
}