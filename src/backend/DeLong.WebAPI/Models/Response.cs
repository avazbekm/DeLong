namespace DeLong.WebAPI.Models;

#pragma warning disable // warninglarni o'chirish uchun
public class Response
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public Object Data { get; set; }
}
