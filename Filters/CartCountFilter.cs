using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;

namespace QLbansach_tutorial.Filters;

public class CartCountFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            var accountIdClaim = context.HttpContext.User.FindFirst("AccountId");
            if (accountIdClaim != null && int.TryParse(accountIdClaim.Value, out int userId))
            {
                var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                var count = await dbContext.Carts
                    .Where(c => c.MaKh == userId)
                    .SumAsync(c => c.Sl ?? 0);
                context.HttpContext.Items["CartCount"] = count;
            }
            else
            {
                context.HttpContext.Items["CartCount"] = 0;
            }
        }
        else
        {
            context.HttpContext.Items["CartCount"] = 0;
        }

        await next();
    }
} 