using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

public static class TokeExtensions
{
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string token)
    {
        return builder.UseMiddleware<TokenMiddleware>(token);
    }
};

public class TokenMiddleware
{
    private readonly RequestDelegate m_next;
    private readonly string m_token;


    public TokenMiddleware(RequestDelegate next, string token)
    {
        m_next = next;
        m_token = token;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token  = context.Request.Query["token"];
        
        if(m_token != token)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid token");
        }
        else
            await m_next.Invoke(context);
    }

};