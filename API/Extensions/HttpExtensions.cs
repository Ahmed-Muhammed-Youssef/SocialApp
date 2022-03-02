using API.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader paginationHeader)
        {
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
