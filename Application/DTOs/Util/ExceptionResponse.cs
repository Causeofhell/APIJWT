using System.Net;

namespace Application.DTOs.Util
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
}
