using Microsoft.AspNetCore.Http;

namespace DeLong.Application.Helpers;

#pragma warning disable  // warninglarni o'chirish uchun
public class HttpContextHelper
{
    private static IHttpContextAccessor HttpContextAccessor { get; set; }
    public static HttpContext HttpContext => HttpContextAccessor?.HttpContext;
    public static IHeaderDictionary ResponseHeaders => HttpContext?.Response?.Headers;
    private static HttpContext Context = HttpContextAccessor.HttpContext;
    public static long GetUserId => long.Parse(Context?.User?.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
}
