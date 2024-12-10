using DeLong.WebAPI.Models;
using DeLong.Application.Exceptions;

namespace DeLong.WebAPI.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate request;
    private readonly ILogger<ExceptionHandlerMiddleware> logger;
    public ExceptionHandlerMiddleware(RequestDelegate request, ILogger<ExceptionHandlerMiddleware> logger)
    {
        this.logger = logger;
        this.request = request;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.request.Invoke(context);
        }
        catch (NotFoundException ex)
        {
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = ex.StatusCode,
                Message = ex.Message,
            });
        }
        catch (AlreadyExistException ex)
        {
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = ex.StatusCode,
                Message = ex.Message,
            });
        }
        catch (Exception ex)
        {
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = 500,
                Message = ex.Message,
            });
            logger.LogError(ex.ToString());
        }
    }

}
