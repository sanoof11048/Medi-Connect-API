using System.Security.Claims;

namespace Medi_Connect_API.middleware
{
    public class GetUserIdMiddleWare
    {
        private readonly RequestDelegate _next;
        public GetUserIdMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.User.Identity?.IsAuthenticated== true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && Guid.TryParse(idClaim.Value, out var userId))
                    context.Items["UserId"] = userId;
                
            }
            await _next(context);
        }
    }
}
